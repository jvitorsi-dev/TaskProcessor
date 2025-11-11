using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Models;
using TaskProcessor.Domain.Models;

public class EmailJobWorker : BackgroundService
{
    private readonly ILogger<EmailJobWorker> _logger;
    private readonly IMessageQueue _messageQueue;
    private readonly IMongoJobRepository _jobRepository;
    private readonly string _queueName = "job_email";

    public EmailJobWorker(
        ILogger<EmailJobWorker> logger,
        IMessageQueue messageQueue,
        IMongoJobRepository jobRepository)
    {
        _logger = logger;
        _messageQueue = messageQueue;
        _jobRepository = jobRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _messageQueue.ConsumeAsync(_queueName, async message =>
        {
            Job job = JsonSerializer.Deserialize<Job>(message)!;
            job.Tentativas++;

            try
            {
                if (!job.Tipo.Equals("EmailJob", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning($"[EmailJobWorker] Tipo inválido: {job.Tipo}");
                    return;
                }

                _logger.LogInformation($"[EmailJobWorker] Processando job: {job.Id}, Dados: {job.Dados}");
                job.Status = JobStatus.EmProcessamento;
                await _jobRepository.UpdateStatus(job.Id, job.Status, job.Tentativas);
                await Task.Delay(1000); // Simula envio de e-mail
                job.Status = JobStatus.Concluido;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao processar job {job.Id}");
                job.Status = job.Tentativas >= 3 ? JobStatus.Erro : JobStatus.Pendente;
            }

            await _jobRepository.UpdateStatus(job.Id, job.Status, job.Tentativas);
        });

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

}
