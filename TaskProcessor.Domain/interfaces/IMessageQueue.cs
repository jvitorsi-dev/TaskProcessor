using TaskProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskProcessor.Domain.Interfaces
{
    public interface IMessageQueue
    {
        Task PublishAsync(string queueName, string message);
        Task ConsumeAsync(string queueName, Func<string, Task> onMessageReceived);
    }
}
