using RabbitMQ.Service.Bus.Events.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Service.Bus.CommandBus
{
    public interface IIntegrationEventHandler<in T> where T : IntegrationEvent
    {
        Task Handle(T @event);
    }
    public interface IIntegrationEventHandler
    {
    }
}