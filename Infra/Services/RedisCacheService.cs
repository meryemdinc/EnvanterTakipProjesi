using Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class RedisCacheService(IDistributedCache distributedCache) : ICacheService
    {
        public async Task<T?> GetAsync<T>(string key)
        {
            var cachedData = await distributedCache.GetStringAsync(key);
            if (string.IsNullOrEmpty(cachedData))
                return default; // Cache'de yoksa null dön

            return JsonSerializer.Deserialize<T>(cachedData);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromHours(1), // Varsayılan 1 saat yaşar
                SlidingExpiration = slidingExpireTime // İstenirse: Kullanıldıkça süresi uzar
            };

            var serializedData = JsonSerializer.Serialize(value);
            await distributedCache.SetStringAsync(key, serializedData, options);
        }

        public async Task RemoveAsync(string key)
        {
            await distributedCache.RemoveAsync(key);
        }
    }
}