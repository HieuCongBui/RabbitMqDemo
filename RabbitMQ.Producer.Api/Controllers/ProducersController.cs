using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Contract.Abstractions.Enumeractions;
using RabbitMQ.Contract.Abstractions.IntergrationEvents;
using static RabbitMQ.Contract.Abstractions.IntergrationEvents.DomainEvent;

namespace RabbitMQ.Producer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducersController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        public ProducersController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet(Name = "pubslish-sms-notification")]
        public async Task<IActionResult> PublishSmsNotificationEvent()
        {
            await _publishEndpoint.Publish(new DomainEvent.SmSNotificationEvent()
            {
                Id = Guid.NewGuid(),
                TimeStamp = DateTimeOffset.UtcNow,
                Name = "Sms Notification",
                Description = "This is a test sms notification",
                Type = NotificationType.sms,
                TransactionId = Guid.NewGuid()
            });
            return Accepted();
        }
    }
}
