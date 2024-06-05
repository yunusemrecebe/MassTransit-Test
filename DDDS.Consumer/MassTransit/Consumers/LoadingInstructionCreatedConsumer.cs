using DDDS.Test.WebAPI.Constants;
using DDDS.Test.WebAPI.Models.Entities;
using DDDS.Test.WebAPI.Models.Interface;
using MassTransit;

namespace DDDS.Consumer.MassTransit.Consumers
{
    public class LoadingInstructionCreatedConsumer : IConsumer<IQueueMessage>
    {
        private static Dictionary<int,List<IQueueMessage>> _cache = new Dictionary<int, List<IQueueMessage>>();

        public async Task Consume(ConsumeContext<IQueueMessage> context)
        {
            IQueueMessage message = context.Message;
            Console.WriteLine($"Data consumed {message.Name}");

            if (_cache.TryGetValue(message.CityCode, out List<IQueueMessage>? cachedListByCity))
            {
                cachedListByCity.Add(message);
            }
            else
            {
                var list = new List<IQueueMessage>() { message };
                _cache.Add(message.CityCode, list);
            }

            bool isThresholdExceeded = EvaluateThresholdStates(5, message.CityCode);

            if (isThresholdExceeded)
                await SendQueueMessages(context);
        }

        private bool EvaluateThresholdStates(int thresholdCount, int cacheKey)
        {
            if (thresholdCount <= _cache.Where(x => x.Key == cacheKey).Select(x => x.Value).First().Count())
                return true;

            return false;
        }

        private async Task SendQueueMessages(ConsumeContext<IQueueMessage> context)
        {
            IQueueMessage message = context.Message;
            IThresholdExceededMessage thresholdExceededMessage = new ThresholdExceededMessage { CityCode = message.CityCode };

            //List<IQueueMessage> data = _cache.Where(x => x.Key == message.CityCode).Select(x => x.Value).First();

            string uri = $"{RabbitMQConstants.Uri}/{RabbitMQConstants.Events.LoadingInstructionThresholdExceeded}";
            Uri endPointUri = new Uri(uri);
            ISendEndpoint ep = await context.GetSendEndpoint(endPointUri);
            await ep.Send(thresholdExceededMessage);
            
            _cache.Remove(message.CityCode);

            Console.WriteLine($"Datalist is published");
        }
    }
}
