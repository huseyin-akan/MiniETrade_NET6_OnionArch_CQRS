using Microsoft.Extensions.Caching.Memory;
using MiniETrade.Application.Abstractions.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure.Services.Caching
{
    public class InMemoryCachingService : IInMemoryCachingService
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCachingService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T? Get<T>(string key)
        {
            //return _memoryCache.Get<T>(key); //Bunu da kullanabilirsin, aşağıdaki daha korunaklı gibi.

            if (_memoryCache.TryGetValue(key, out T? result)) return result;
            return default;
        }

        public void Set<T>(string key, T value)
        {
            _memoryCache.Set(key, value, options: new()
            {
                AbsoluteExpiration = DateTime.UtcNow.AddHours(1),   //1 saat boyunca cache'de kalıcak, 1 saat sonra mutlaka silinecek.
                SlidingExpiration = TimeSpan.FromMinutes(20)        //Her 20 dakikada bir istek gelmezse 20 dakikanın sonunda bu data silinecek.
            }); ;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
