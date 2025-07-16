using MediatR;
using RabbitMQ.Contract.Abstractions.IntergrationEvents;

namespace RabbitMQ.Consumer.Api.UserCases.Events
{
    public class SendSmsEventComsumerHandler : IRequestHandler<DomainEvent.SmSNotificationEvent>
    {
        private readonly ILogger<SendSmsEventComsumerHandler> _logger;
        public SendSmsEventComsumerHandler(ILogger<SendSmsEventComsumerHandler> logger)
        {
            _logger = logger;
        }
        public async Task Handle(DomainEvent.SmSNotificationEvent request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sms Notification Event Received: {message}", request);
        }
    }
}
