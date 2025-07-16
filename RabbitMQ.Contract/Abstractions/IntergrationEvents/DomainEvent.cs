using RabbitMQ.Contract.Abstractions.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Contract.Abstractions.IntergrationEvents
{
    public static class DomainEvent
    {
        public record EmailNotificationEvent : INotificationEvent
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Type { get; set; } // sms|email
            public Guid TransactionId { get; set; }
            public Guid Id { get; set; }
            public DateTimeOffset TimeStamp { get; set; }
        }

        public record SmSNotificationEvent : INotificationEvent
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Type { get; set; } // sms|email
            public Guid TransactionId { get; set; }
            public Guid Id { get; set; }
            public DateTimeOffset TimeStamp { get; set; }
        }
    }
}
