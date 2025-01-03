using DDDS.Test.WebAPI.Constants;
using LGW.MessageDistributor.Messagebus.Contract.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace DDDS.Test.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestMessageQueueController : ControllerBase
    {
        public readonly IBus _bus;
        public TestMessageQueueController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost("InsertLoadingInstruction")]
        public async Task<IActionResult> InsertLoadingInstruction(LoadingInstructionCreatedEventModel queueMessage, CancellationToken cancellationToken)
        {
            string uri = $"{RabbitMQConstants.Uri.Replace("http", "amqp")}/{RabbitMQConstants.Events.LoadingInstructionCreated}";

            Uri endPointUri = new Uri(uri);
            ISendEndpoint ep = await _bus.GetSendEndpoint(endPointUri);
            await ep.Send(queueMessage, cancellationToken);

            return Ok();
        }
        
        [HttpPost("LoadingInstructionApplied")]
        public async Task<IActionResult> LoadingInstructionApplied(LoadingInstructionAppliedEventModel queueMessage, CancellationToken cancellationToken)
        {
            string uri = $"{RabbitMQConstants.Uri}/{RabbitMQConstants.Events.LoadingInstructionApplied}";

            Uri endPointUri = new Uri(uri);
            ISendEndpoint ep = await _bus.GetSendEndpoint(endPointUri);
            await ep.Send(queueMessage, cancellationToken);

            return Ok();
        }

        [HttpPost("InsertLoadingInstructionThresholdExceeded")]
        public async Task<IActionResult> InsertLoadingInstructionThresholdExceeded(LoadingInstructionThresholdExceededEventModel queueMessage, CancellationToken cancellationToken)
        {
            await _bus.Publish(queueMessage, x => x.SetRoutingKey($"loadinginstructionexceeded.{queueMessage.CityCode}"), cancellationToken);
            return Ok();
        }

    }
}
