using DDDS.Test.WebAPI.Models.Interface;
using MassTransit;

namespace DDDS.Consumer.MassTransit.Consumers
{
    public class LoadingInstructionCreatedConsumer : IConsumer<IQueueMessage>
    {
        public Task Consume(ConsumeContext<IQueueMessage> context)
        {
            Console.WriteLine($"Data consumed {context.Message.Name}");
            return Task.CompletedTask;
        }
    }
}
