using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MiniETrade.Application.Common.Abstractions.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Behaviours
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICachableRequest
    {
        private readonly IDistributedCachingService _cachingService;
        private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

        public CachingBehavior(IDistributedCachingService cachingService, ILogger<CachingBehavior<TRequest, TResponse>> logger)
        {
            _cachingService = cachingService;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request.BypassCache) return await next();

            TResponse response;
            var cachedResponse = _cachingService.Get<TResponse>(request.CacheKey);
            if (cachedResponse is not null)
            {
                response = cachedResponse;
                _logger.LogInformation($"Fetched from Cache -> {request.CacheKey}");
            }
            else
            {
                response = await GetResponseAndAddToCache(request, next, cancellationToken);
            }

            return response;
        }

        private async Task<TResponse> GetResponseAndAddToCache(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response = await next();

            _cachingService.Set(request.CacheKey, response);

            _logger.LogInformation($"Added to Cache -> {request.CacheKey}");

            if (request.CacheGroupKey != null)
                await AddCacheKeyToGroup(request, slidingExpiration, cancellationToken);

            return response;
        }

        private async Task AddCacheKeyToGroup(TRequest request, TimeSpan slidingExpiration, CancellationToken cancellationToken)
        {
            byte[]? cacheGroupCache = await _cachingService.GetAsync(key: request.CacheGroupKey!, cancellationToken);
            HashSet<string> cacheKeysInGroup;
            if (cacheGroupCache != null)
            {
                cacheKeysInGroup = JsonSerializer.Deserialize<HashSet<string>>(Encoding.Default.GetString(cacheGroupCache))!;
                if (!cacheKeysInGroup.Contains(request.CacheKey))
                    cacheKeysInGroup.Add(request.CacheKey);
            }
            else
                cacheKeysInGroup = new HashSet<string>(new[] { request.CacheKey });
            byte[] newCacheGroupCache = JsonSerializer.SerializeToUtf8Bytes(cacheKeysInGroup);

            byte[]? cacheGroupCacheSlidingExpirationCache = await _cachingService.GetAsync(
                key: $"{request.CacheGroupKey}SlidingExpiration",
                cancellationToken
            );
            int? cacheGroupCacheSlidingExpirationValue = null;
            if (cacheGroupCacheSlidingExpirationCache != null)
                cacheGroupCacheSlidingExpirationValue = Convert.ToInt32(Encoding.Default.GetString(cacheGroupCacheSlidingExpirationCache));
            if (cacheGroupCacheSlidingExpirationValue == null || slidingExpiration.TotalSeconds > cacheGroupCacheSlidingExpirationValue)
                cacheGroupCacheSlidingExpirationValue = Convert.ToInt32(slidingExpiration.TotalSeconds);
            byte[] serializeCachedGroupSlidingExpirationData = JsonSerializer.SerializeToUtf8Bytes(cacheGroupCacheSlidingExpirationValue);

            DistributedCacheEntryOptions cacheOptions =
                new() { SlidingExpiration = TimeSpan.FromSeconds(Convert.ToDouble(cacheGroupCacheSlidingExpirationValue)) };

            await _cachingService.SetAsync(key: request.CacheGroupKey!, newCacheGroupCache, cacheOptions, cancellationToken);
            _logger.LogInformation($"Added to Cache -> {request.CacheGroupKey}");

            await _cachingService.SetAsync(
                key: $"{request.CacheGroupKey}SlidingExpiration",
                serializeCachedGroupSlidingExpirationData,
                cacheOptions,
                cancellationToken
            );
            _logger.LogInformation($"Added to Cache -> {request.CacheGroupKey}SlidingExpiration");
        }
    }
}