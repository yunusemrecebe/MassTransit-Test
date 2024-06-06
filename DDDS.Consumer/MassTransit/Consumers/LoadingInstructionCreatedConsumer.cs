using DDDS.Test.WebAPI.Constants;
using DDDS.Test.WebAPI.Models.Entities;
using DDDS.Test.WebAPI.Models.Interface;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using System.Net.Http.Headers;
using System.Text.Json;

namespace DDDS.Consumer.MassTransit.Consumers
{
    public class LoadingInstructionCreatedConsumer : IConsumer<QueueMessage>
    {

        public async Task Consume(ConsumeContext<QueueMessage> context)
        {
            Console.WriteLine($"Data consumed {context.Message.CityCode}");

            QueueMessage message = context.Message;
            string cacheKey = message.CityCode.ToString();
            
            List<QueueMessage> cachedData = new() { message };
            string getCacheResult = await GetCache(message.CityCode.ToString());

            if (!string.IsNullOrEmpty(getCacheResult))
                cachedData.AddRange(JsonSerializer.Deserialize<List<QueueMessage>>(getCacheResult));

            await SaveCache(cacheKey, cachedData);

            bool isThresholdExceeded = 5 <= cachedData.Count();

            if (isThresholdExceeded)
                await SendQueueMessages(context);

        }

        private async Task SendQueueMessages(ConsumeContext<QueueMessage> context)
        {
            IQueueMessage message = context.Message;
            ThresholdExceededMessage thresholdExceededMessage = new ThresholdExceededMessage { CityCode = message.CityCode };

            string uri = $"{RabbitMQConstants.Uri}/{RabbitMQConstants.Events.LoadingInstructionThresholdExceeded}";
            Uri endPointUri = new Uri(uri);
            ISendEndpoint ep = await context.GetSendEndpoint(endPointUri);
            await ep.Send(thresholdExceededMessage);
            
            Console.WriteLine($"Datalist is published");
        }

        private async Task SaveCache(string cacheKey, List<QueueMessage> queueMessage)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"http://localhost:5204/Cache/LoadingInstructions?cacheKey={cacheKey}");
            var content = new StringContent(JsonSerializer.Serialize(queueMessage), null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();
        }

        private async Task<string> GetCache(string cacheKey)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:5204/Cache/LoadingInstructions?cacheKey={cacheKey}");
            
            var response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

    }
}
