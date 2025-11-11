
using TaskProcessor.Models;

namespace TaskProcessor.Application.Interfaces
{
    public interface IReportJobService
    {
        Task<string> CriarJob(Job request);
        Task<Job> ObterPorId(string id);
    }
}
