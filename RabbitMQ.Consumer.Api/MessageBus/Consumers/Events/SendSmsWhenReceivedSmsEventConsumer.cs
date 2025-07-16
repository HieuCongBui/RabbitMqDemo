using RabbitMQ.Contract.Abstractions.IntergrationEvents;
using RabbitMQ.Consumer.Api.Abstractions.Messages;
using MediatR;

namespace RabbitMQ.Consumer.Api.MessageBus.Consumers.Events
{
    public class SendSmsWhenReceivedSmsEventConsumer : Comsumer<DomainEvent.SmSNotificationEvent>
    {
        public SendSmsWhenReceivedSmsEventConsumer(ISender sender):base(sender)
        {     
        }
    }
}
