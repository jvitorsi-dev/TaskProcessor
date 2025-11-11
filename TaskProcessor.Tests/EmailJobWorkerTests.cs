using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using TaskProcessor.Application.DTOs;
using TaskProcessor.Application.Interfaces;
using TaskProcessor.Domain.interfaces;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Models;
using TaskProcessor.Models;
using Xunit;

namespace TaskProcessor.Tests
{
    public class EmailJobWorkerTests
    {
        [Fact]
        public async Task Deve_Consumir_Mensagem_Valida_E_Atualizar_Status()
        {
            // Arrange
            var job = new Job
            {
                Tipo = "EmailJob",
                Dados = "teste@email.com",
                Status = JobStatus.Pendente,
                Tentativas = 0,
                DataCriacao = DateTime.UtcNow,
            };

            var mockQueue = new Mock<IMessageQueue>();
            var mockLogger = new Mock<ILogger<EmailJobWorker>>();
            var mockRepository = new Mock<IMongoJobRepository>();

            var worker = new EmailJobWorker(mockLogger.Object, mockQueue.Object, mockRepository.Object);

            mockQueue.Setup(q => q.ConsumeAsync(It.IsAny<string>(), It.IsAny<Func<string, Task>>()))
                .Callback<string, Func<string, Task>>(async (queueName, callback) =>
                {
                    var json = JsonSerializer.Serialize(job);
                    await callback(json);
                })
                .Returns(Task.CompletedTask);

            // Act
            await worker.StartAsync(CancellationToken.None);

            // Assert
            mockRepository.Verify(r => r.UpdateStatus(
                job.Id,
                JobStatus.EmProcessamento,
                It.IsAny<int>()), Times.Once);
        }
    }
}