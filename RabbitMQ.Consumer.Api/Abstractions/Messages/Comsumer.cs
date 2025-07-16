using MassTransit;
using MediatR;
using RabbitMQ.Contract.Abstractions.Messages;

namespace RabbitMQ.Consumer.Api.Abstractions.Messages
{
    public abstract class Comsumer<TEntity> : IConsumer<TEntity> 
        where TEntity : class, INotificationEvent
    {
        private readonly ISender _sender;
        protected Comsumer(ISender sender)
        {
            _sender = sender;
        }
        public async Task Consume(ConsumeContext<TEntity> context)
        {
            await _sender.Send(context.Message);
        }
    }
}
