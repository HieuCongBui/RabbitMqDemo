using MediatR;
using RabbitMQ.Consumer.Api.Abstractions.Messages;
using RabbitMQ.Contract.Abstractions.IntergrationEvents;

namespace RabbitMQ.Consumer.Api.MessageBus.Consumers.Events
{
    public class SendEmailWhenReceivedEmailEventConsumer : Comsumer<DomainEvent.EmailNotificationEvent>
    {
        public SendEmailWhenReceivedEmailEventConsumer(ISender sender) : base(sender)
        {
        }
    }
}
