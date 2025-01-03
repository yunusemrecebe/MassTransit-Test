using Asis.Framework.Monitoring.Interfaces;
using LGW.MessageDistributor.Messagebus.Contract.Events;
using MassTransit;

namespace DDDS.Consumer.MassTransit.Consumers
{
    public class LoadingInstructionCreatedConsumer : IConsumer<LoadingInstructionCreatedEventModel>
    {
        private readonly IMessageBus MessageBus;

        public LoadingInstructionCreatedConsumer(IMessageBus messageBus)
        {
            MessageBus = messageBus;
        }
        
        public async Task Consume(ConsumeContext<LoadingInstructionCreatedEventModel> context)
        {
            try
            {
                LoadingInstructionCreatedEventModel message = context.Message;
                await SendThresholdExceededMessage(context);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        
        private async Task SendThresholdExceededMessage(ConsumeContext<LoadingInstructionCreatedEventModel> context)
        {
            LoadingInstructionCreatedThresholdExceededEventModel thresholdExceededMessage = new LoadingInstructionCreatedThresholdExceededEventModel
            {
                CityCode = context.Message.CityCode,
                CorrelationId = context.Message.CorrelationId
            };

            await MessageBus.SendAsync(thresholdExceededMessage);
            // await QueueHelper.PublishMessage(context, thresholdExceededMessage);
        }
    }
}
