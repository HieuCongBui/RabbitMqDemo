using RabbitMQ.Service.Bus.CommandBus;
using RabbitMQ.Service.Bus.Events.Core;

namespace RabbitMQ.Service.Bus.RabbitMq
{
    public interface IEventBusRabbitMQ
    {
        void Publish (IntegrationEvent @event);
        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;
    }
}
