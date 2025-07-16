using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Contract.Abstractions.Messages
{
    [ExcludeFromTopology]
    public interface IMessage : IRequest
    {
        public Guid Id { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}
