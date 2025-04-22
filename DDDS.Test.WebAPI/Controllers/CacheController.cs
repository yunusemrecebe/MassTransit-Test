using DDDS.Test.WebAPI.Repository;
using LGW.MessageDistributor.Messagebus.Contract.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace DDDS.Test.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CacheController : ControllerBase
    {
        IDistributedCache _distributedCache;

        public CacheController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet("LoadingInstructions")]
        public async Task<IActionResult> GetLoadingInstructionCache(string cacheKey)
        {
            string? result = await _distributedCache.GetStringAsync($"LoadingInstructions_{cacheKey}");
            return Ok(result);
        }

        [HttpPost("LoadingInstructions")]
        public async Task<IActionResult> InsertLoadingInstructionCache(string cacheKey, List<LoadingInstructionCreatedEventModel> queueMessage)
        {
            await _distributedCache.AddOrUpdate($"LoadingInstructions_{cacheKey}", queueMessage);
            return Ok(queueMessage);
        }

        [HttpDelete("LoadingInstructions")]
        public async Task<IActionResult> RemoveLoadingInstructionCache(string cacheKey)
        {
            await _distributedCache.RemoveAsync($"LoadingInstructions_{cacheKey}");
            return Ok();
        }
    }
}
