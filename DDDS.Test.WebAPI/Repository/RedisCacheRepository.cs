using Microsoft.Extensions.Caching.Distributed;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace DDDS.Test.WebAPI.Repository
{
    public static class RedisCacheRepository
    {
        public async static Task AddOrUpdate<T>(this IDistributedCache cache, string cacheKey, T entity) where T : class
        {
            string? cachedValue = await cache.GetStringAsync(cacheKey);
            string serializedEntity = JsonSerializer.Serialize(entity);

            if (cachedValue is null)
            {
                await cache.SetStringAsync(cacheKey, serializedEntity);
            }
            else
            {
                await cache.RemoveAsync(cacheKey);
                await cache.SetStringAsync(cacheKey, serializedEntity);
            }
        }

        public async static Task Update<T>(this IDistributedCache cache, string cacheKey, T entity) where T : class
        {
            string serializedEntity = JsonSerializer.Serialize(entity);

            await cache.RemoveAsync(cacheKey);
            await cache.SetStringAsync(cacheKey, serializedEntity);
        }

    }
}
