using DDDS.Test.WebAPI.Constants;
using DDDS.Test.WebAPI.Models.Entities;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace DDDS.Test.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestMessageQueueController : ControllerBase
    {
        public readonly IBus _bus;
        private static List<QueueMessage> queueMessages = new List<QueueMessage>();

        public TestMessageQueueController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> PublishMessage(QueueMessage queueMessage)
        {
            int messageCount = queueMessages.Count();

            if (++messageCount == 5)
            {
                string uri = $"{RabbitMQConstants.Uri}/{RabbitMQConstants.Events.LoadingInstructionThresholdExceeded}";

                Uri endPointUri = new Uri(uri);
                ISendEndpoint ep = await _bus.GetSendEndpoint(endPointUri);

                await ep.SendBatch(queueMessages);
                queueMessages.Clear();

                return Ok($"{messageCount}");
            }
            else
            {
                queueMessages.Add(queueMessage);

                return Ok($"{messageCount}");
            }
        }
    }
}
