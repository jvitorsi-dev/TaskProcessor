using TaskProcessor.Application.Interfaces;
using TaskProcessor.Models;
using Newtonsoft.Json;
using TaskProcessor.Application.interfaces;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.interfaces;

namespace TaskProcessor.Domain.Services.Email
{
    public class EmailJobService : IJobService
    {
        private readonly IMessageQueue _messageQueue;
        private readonly IMongoJobRepository _repository;
        public string Tipo => "EmailJob";

        public EmailJobService(IMessageQueue messageQueue, IMongoJobRepository repository)
        {
            _messageQueue = messageQueue;
            _repository = repository;
        }

        public async Task<string> CriarJob(Job job)
        {
            try
            {
                await _repository.Add(job);

                var json = JsonConvert.SerializeObject(job);

                await _messageQueue.PublishAsync("job_email", json);

                return job.Id;
            }
            catch(Exception ex)
            {
                var a = ex;
                return ex.Message;
            }
        }

        public async Task<Job> ObterPorId(string id) => await _repository.GetById(id);
    }
}
