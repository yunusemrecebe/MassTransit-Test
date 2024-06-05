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
        public TestMessageQueueController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> InsertLoadingInstruction(QueueMessage queueMessage, CancellationToken cancellationToken)
        {
            //queueMessage MSSQL YuklemeTalimatlari tablosuna Insert Et

            string uri = $"{RabbitMQConstants.Uri}/{RabbitMQConstants.Events.LoadingInstructionCreated}";

            Uri endPointUri = new Uri(uri);
            ISendEndpoint ep = await _bus.GetSendEndpoint(endPointUri);
            await ep.Send(queueMessage, cancellationToken);

            return Ok();
        }
    }
}
