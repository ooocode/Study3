using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Study.Service
{
    public class CacheService
    {
        private readonly IDistributedCache cache;

        public CacheService(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task SetAsync(string key, object obj,TimeSpan? timeSpan)
        {
            if (obj != null)
            {
                var str = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                await cache.SetStringAsync(key, str, new DistributedCacheEntryOptions {
                    AbsoluteExpirationRelativeToNow = timeSpan ?? TimeSpan.FromMinutes(5)
                });
            }
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
            return obj;
        }

        public async Task RemoveAsync(string key)
        {
            await cache.RemoveAsync(key);
        }

        public Task SetAsync(string keyAddOrUpdateAdvertisement, string addvertisementText)
        {
            throw new NotImplementedException();
        }
    }
}
