using DDDS.Test.WebAPI.Models.Interface;
using MassTransit;

namespace DDDS.Consumer.MassTransit.Consumers
{
    public class LoadingInstructionThresholdExceededConsumer : IConsumer<IQueueMessage>
    {
        public Task Consume(ConsumeContext<IQueueMessage> context)
        {
            Console.WriteLine("Dataları çektim");
            Console.WriteLine($"Data: {context.Message.Name}");
            Console.WriteLine("Vallahi protokolü de oluşturdum");
            Console.WriteLine("Bir de istek attım");

            return Task.CompletedTask;
        }
    }
}
