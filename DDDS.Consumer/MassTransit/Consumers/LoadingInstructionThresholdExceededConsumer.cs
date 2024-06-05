using DDDS.Test.WebAPI.Models.Entities;
using MassTransit;
using System.Text.Json;

namespace DDDS.Consumer.MassTransit.Consumers
{
    public class LoadingInstructionThresholdExceededConsumer : IConsumer<ThresholdExceededMessage>
    {
        public async Task Consume(ConsumeContext<ThresholdExceededMessage> context)
        {
            var message = context.Message;
            string cacheKey = message.CityCode.ToString();

            Console.WriteLine($"Dataları çektim! cacheKey: {cacheKey}");

            List<QueueMessage> cachedData = null;
            string getCacheResult = await GetCache(cacheKey);

            if (!string.IsNullOrEmpty(getCacheResult))
                cachedData = JsonSerializer.Deserialize<List<QueueMessage>>(getCacheResult);

            var abc = cachedData;

            await RemoveCache(cacheKey);
        }


        private async Task<string> GetCache(string cacheKey)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:5204/Cache/LoadingInstructions?cacheKey={cacheKey}");

            var response = await client.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }


        private async Task RemoveCache(string cacheKey)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Delete, $"http://localhost:5204/Cache/LoadingInstructions?cacheKey={cacheKey}");
            var response = await client.SendAsync(request);
        }
    }
}
