using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using MiniETrade.Application.Common.Abstractions.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure.Services.Caching.Redis
{
    public class RedisCachingService : IDistibutedCachingService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;
        private readonly DistributedCacheEntryOptions _options;

        public RedisCachingService(IDistributedCache distributedCache, IConfiguration configuration)
        {
            _distributedCache = distributedCache;
            _configuration = configuration;
            _options = new()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Redis:Options:AbsoluteExpiration"])),
                SlidingExpiration = TimeSpan.FromMinutes(Convert.ToDouble(_configuration["Redis:Options:SlidingExpiration"]))
            };
            var asd = "asd";
        }

        public T? Get<T>(string key)
        {
            byte[] byteResult = _distributedCache.Get(key);
            var json = Encoding.UTF8.GetString(byteResult);
            return JsonSerializer.Deserialize<T>(json);
        }

        public string GetString(string key)
        {
            return _distributedCache.GetString(key);
        }

        public void Set<T>(string key, T value)
        {
            if (value == null) return;

            var json = JsonSerializer.Serialize(value);

            _distributedCache.Set(key, Encoding.UTF8.GetBytes(json), options : _options);
        }

        public void SetString(string key, string value)
        {
            _distributedCache.SetString(key, value);
        }
    }
}
