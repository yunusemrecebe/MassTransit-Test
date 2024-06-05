using DDDS.Test.WebAPI.Models.Interface;
using MassTransit;

namespace DDDS.Consumer.MassTransit.Consumers
{
    public class LoadingInstructionThresholdExceededConsumer : IConsumer<IThresholdExceededMessage>
    {
        public Task Consume(ConsumeContext<IThresholdExceededMessage> context)
        {
            Console.WriteLine($"Dataları çektim! Data: {context.Message.CityCode}");

            return Task.CompletedTask;
        }
    }
}
