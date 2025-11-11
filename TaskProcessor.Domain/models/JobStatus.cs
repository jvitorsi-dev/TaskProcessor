using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProcessor.Domain.Models
{
    public enum JobStatus
    {
        Pendente,
        EmProcessamento,
        Concluido,
        Erro
    }
}
