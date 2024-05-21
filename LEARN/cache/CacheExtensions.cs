namespace LEARN.cache
{
    public static class CacheExtensions
    {
        public static async Task<T> GetAsync<T>(this ICacheManager cacheManager, string key, Func<Task<T>> acquire)
       where T : class
        {
            return await GetAsync(cacheManager, key, 60, acquire);
        }

        public static async Task<T> GetAsync<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<Task<T>> acquire)
            where T : class
        {
            var result = await cacheManager.Get<T>(key);

            if (result != null)
                return result;

            result = await acquire();

            if (result != null && cacheTime > 0)
            {
                await cacheManager.Set(key, result, TimeSpan.FromMinutes(cacheTime));
            }

            return result;
        }

        public static async Task<T> GetOrAddAsync<T>(this ICacheManager cacheManager, string key, Func<Task<T>> action,
            TimeSpan? expiryTime = null) where T : class
        {
            var result = await cacheManager.Get<T>(key);

            if (result != null)
            {
                return result;
            }

            var cacheTime = expiryTime ?? TimeSpan.FromMinutes(1440);

            result = await action();

            await cacheManager.Set(key, result, cacheTime);

            return result;
        }
    }
}
