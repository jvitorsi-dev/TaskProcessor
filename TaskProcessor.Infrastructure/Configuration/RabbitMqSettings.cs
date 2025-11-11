using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProcessor.Infrastructure.Configuration
{
    public class RabbitMqSettings
    {
        public string HostName { get; set; } = string.Empty;
        public List<QueueDefinition> Queues { get; set; } = new();
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class QueueDefinition
    {
        public string QueueName { get; set; } = string.Empty;
    }
}
