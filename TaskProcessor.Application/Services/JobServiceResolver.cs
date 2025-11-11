using TaskProcessor.Application.Interfaces;
using TaskProcessor.Domain.interfaces;

namespace Application.Services
{
    public class JobServiceResolver : IJobServiceResolver
    {
        private readonly IDictionary<string, IJobService> _services;

        public JobServiceResolver(IEnumerable<IJobService> services)
        {
            _services = new Dictionary<string, IJobService>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var service in services)
            {
                _services.Add(service.Tipo, service); 
            }
        }

        public IJobService Resolver(string tipoJob)
        {
            if (_services.TryGetValue(tipoJob, out var servico))
                return servico;
            throw new InvalidOperationException($"Tipo de job não suportado: {tipoJob}");
        }
    }
}
