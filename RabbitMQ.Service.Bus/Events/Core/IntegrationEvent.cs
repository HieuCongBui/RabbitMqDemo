using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Service.Bus.Events.Core
{
    public class IntegrationEvent
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime CreationDate { get; private set; } = DateTime.UtcNow;
    }
}
