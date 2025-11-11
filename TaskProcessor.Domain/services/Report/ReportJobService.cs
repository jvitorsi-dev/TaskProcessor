using TaskProcessor.Application.Models;

using TaskProcessor.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProcessor.Domain.Models;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.interfaces;
namespace TaskProcessor.Domain.Services.Report
{
    public class ReportJobService : IJobService
    {
        private readonly IMessageQueue _messageQueue;
        private readonly IMongoJobRepository _repository;
        public string Tipo => "ReportJob";

        public ReportJobService(IMessageQueue messageQueue, IMongoJobRepository repository)
        {
            _messageQueue = messageQueue;
            _repository = repository;
        }

        public async Task<string> CriarJob(Job request)
        {
            var job = new Job
            {
                Tipo = request.Tipo,
                Dados = request.Dados,
                Status = JobStatus.Pendente,
                Tentativas = 0
            };

            await _repository.Add(job);

            var json = JsonConvert.SerializeObject(job);

            await _messageQueue.PublishAsync("job_report", json);

            return job.Id;
        }

        public async Task<Job> ObterPorId(string id) => await _repository.GetById(id);
    }
}
