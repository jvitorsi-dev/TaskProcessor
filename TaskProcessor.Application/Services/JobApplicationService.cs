using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProcessor.Application.DTOs;
using TaskProcessor.Application.Interfaces;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Models;
using TaskProcessor.Models;

namespace TaskProcessor.Application.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private IJobServiceResolver _jobServiceResolver;
        private IMongoJobRepository _mongoJobRepository;

        public JobApplicationService(IJobServiceResolver jobServiceResolver, IMongoJobRepository mongoJobRepository)
        {
            _jobServiceResolver = jobServiceResolver;
           _mongoJobRepository = mongoJobRepository;
        }

        public async Task<string> CriarJobAsync(JobRequest request)
        {
            if (string.IsNullOrEmpty(request.Tipo)) throw new ArgumentException("Tipo é obrigatório");

            var job = new Job
            {
                Tipo = request.Tipo,
                Dados = request.Dados
            };

            var service = _jobServiceResolver.Resolver(request.Tipo);
            await service.CriarJob(job);
            return job.Id;
        }

        public async Task<Job> ObterPorIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Id é obrigatório");
            var job = await _mongoJobRepository.GetById(id);
            if (job == null) throw new KeyNotFoundException("Job não encontrado");
            return job;
        }
    }

}
