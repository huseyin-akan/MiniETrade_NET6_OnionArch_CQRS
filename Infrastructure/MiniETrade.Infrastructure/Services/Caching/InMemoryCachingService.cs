using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MiniETrade.Application.Common.Abstractions.Caching;
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
        private readonly IConfiguration _configuration;

        public InMemoryCachingService(IMemoryCache memoryCache, IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
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
                //1 saat boyunca cache'de kalıcak, 1 saat sonra mutlaka silinecek.
                AbsoluteExpiration = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["InMemoryCaching:Options:AbsoluteExpiration"])),

                //Her 20 dakikada bir istek gelmezse 20 dakikanın sonunda bu data silinecek.
                SlidingExpiration = TimeSpan.FromMinutes(Convert.ToDouble(_configuration["InMemoryCaching:Options:SlidingExpiration"]))
            }); 
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
