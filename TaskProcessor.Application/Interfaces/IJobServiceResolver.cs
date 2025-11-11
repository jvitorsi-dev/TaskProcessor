using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProcessor.Domain.interfaces;

namespace TaskProcessor.Application.Interfaces
{
    public interface IJobServiceResolver
    {
        IJobService Resolver(string tipoJob);
    }
}
