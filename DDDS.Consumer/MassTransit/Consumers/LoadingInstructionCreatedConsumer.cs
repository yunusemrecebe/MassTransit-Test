namespace DDDS.Consumer.MassTransit.Consumers
{
    // public class LoadingInstructionCreatedConsumer : IConsumer<LoadingInstructionCreatedEventModel>
    // {
    //     // private readonly IMessageBus MessageBus;
    //     //
    //     // public LoadingInstructionCreatedConsumer(IMessageBus messageBus)
    //     // {
    //     //     MessageBus = messageBus;
    //     // }
    //
    //
    //     public async Task Consume(ConsumeContext<LoadingInstructionCreatedEventModel> context)
    //     {
    //         try
    //         {
    //             LoadingInstructionCreatedEventModel message = context.Message;
    //             await SendThresholdExceededMessage(context);
    //         }
    //         catch (Exception e)
    //         {
    //             throw;
    //         }
    //     }
    //
    //     private async Task SendThresholdExceededMessage(ConsumeContext<LoadingInstructionCreatedEventModel> context)
    //     {
    //         LoadingInstructionCreatedThresholdExceededEventModel thresholdExceededMessage =
    //             new LoadingInstructionCreatedThresholdExceededEventModel
    //             {
    //                 CityCode = context.Message.CityCode,
    //                 CorrelationId = context.Message.CorrelationId
    //             };
    //
    //         bool isCorrelationIdParsed = Guid.TryParse(context.Message.CorrelationId, out Guid correlationId);
    //
    //         ConcurrentDictionary<string, object>
    //             asisMonitoringContextItems = new ConcurrentDictionary<string, object>();
    //         asisMonitoringContextItems.TryAdd("CityId", context.Message.CityCode);
    //
    //         // await MessageBus.SendAsync(thresholdExceededMessage,
    //         //     new AsisMonitoringContextOptions
    //         //     {
    //         //         CorrelationId = isCorrelationIdParsed ? correlationId : null, Endpoint = $"{GetType().Name}",
    //         //         Items = asisMonitoringContextItems
    //         //     });
    //         // await QueueHelper.PublishMessage(context, thresholdExceededMessage);
    //     }
    //
    //     // public event Func<string, Task>? OnMessageReceivedAsync;
    // }
}