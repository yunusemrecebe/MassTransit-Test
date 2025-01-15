namespace LGW.MessageDistributor.MessageBus.Core.Helpers
{
    public static class QueueHelper
    {
        // public async static Task SendMessage<T>(ConsumeContext<T> context, string messageEndpoint, object message) where T : class
        // {
        //     Uri endPointUri = new Uri(messageEndpoint);
        //     ISendEndpoint ep = await context.GetSendEndpoint(endPointUri);
        //     await ep.Send(message);
        // }
        //
        // public async static Task PublishMessage<T>(ConsumeContext<T> context, object message) where T : class
        // {
        //     await context.Publish(message);
        // }
    }
}
