using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LEARN.cache
{
    public class MemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Dispose()
        {
        }

        public Task<T> Get<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            _memoryCache.TryGetValue<T>(key, out var result);

            return Task.FromResult(result);
        }

        public Task Set(string key, object data, TimeSpan? expiry = null, CancellationToken cancellationToken = default)
        {
            if (expiry == null)
                _memoryCache.Set(key, data);
            else
                _memoryCache.Set(key, data, expiry.Value);

            return Task.CompletedTask;
        }

        public Task Remove(string key, CancellationToken cancellationToken = default)
        {
            _memoryCache.Remove(key);

            return Task.CompletedTask;
        }

        public async Task RemoveByPattern(string pattern, CancellationToken cancellationToken = default)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var keysToRemove = (from item in GetKeys<string>(_memoryCache) where regex.IsMatch(item) select item).ToList();

            foreach (var key in keysToRemove)
            {
                await Remove(key, cancellationToken);
            }
        }

        public Task Clear(CancellationToken cancellationToken = default)
        {
            foreach (var key in GetKeys(_memoryCache))
            {
                _memoryCache.Remove(key);
            }

            return Task.CompletedTask;
        }

        private static readonly Func<MemoryCache, object> GetEntriesCollection = Delegate.CreateDelegate(
            typeof(Func<MemoryCache, object>),
            typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetGetMethod(true),
            throwOnBindFailure: true) as Func<MemoryCache, object>;

        public static IEnumerable GetKeys(IMemoryCache memoryCache) =>
            ((IDictionary)GetEntriesCollection((MemoryCache)memoryCache)).Keys;

        public static IEnumerable<T> GetKeys<T>(IMemoryCache memoryCache) => GetKeys(memoryCache).OfType<T>();
    }
}
