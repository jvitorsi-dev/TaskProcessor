using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProcessor.Application.DTOs;
using TaskProcessor.Models;

namespace TaskProcessor.Application.Interfaces
{
    public interface IJobApplicationService
    {
        Task<string> CriarJobAsync(JobRequest request);
        Task<Job> ObterPorIdAsync(string id);
    }
}
