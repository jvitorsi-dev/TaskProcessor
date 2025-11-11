
using TaskProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProcessor.Domain.Models;

namespace TaskProcessor.Domain.Interfaces
{
    public interface IMongoJobRepository
    {
        Task Add(Job job);
        Task UpdateStatus(string jobId, JobStatus status, int? tentativas = null);
        Task<Job> GetById(string id);
    }
}
