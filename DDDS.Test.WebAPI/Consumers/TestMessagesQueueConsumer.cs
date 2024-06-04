using DDDS.Test.WebAPI.Models.Entities;
using DDDS.Test.WebAPI.Models.Interface;
using MassTransit;
using System.Diagnostics;

namespace DDDS.Test.WebAPI.Consumers
{
    public class TestMessagesQueueConsumer : IConsumer<QueueMessage>
    {
        public async Task Consume(ConsumeContext<QueueMessage> context)
        {
            QueueMessage message = context.Message;
            Debug.WriteLine($"Message Received: {message.Name}", DateTime.Now);
        }
    }
}
