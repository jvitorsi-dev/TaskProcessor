using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProcessor.Models;

namespace TaskProcessor.Domain.interfaces
{
    public interface IJobService
    {
        string Tipo { get; }
        Task<string> CriarJob(Job request);
        Task<Job> ObterPorId(string id);
    }
}
