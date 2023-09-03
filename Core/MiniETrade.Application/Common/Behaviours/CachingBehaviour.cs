using MediatR;
using MiniETrade.Application.Common.Abstractions.Caching;
using MiniETrade.Application.Common.Abstractions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ILoggerFactory = MiniETrade.Application.Common.Abstractions.Logging.ILoggerFactory;

namespace MiniETrade.Application.Common.Behaviours
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICachableRequest
    {
        private readonly ICachingService _cachingService;
        private readonly ILoggerService _loggerService; 

        public CachingBehavior(ICachingService cachingService, ILoggerFactory loggerFactory)
        {
            _cachingService = cachingService;
            _loggerService = loggerFactory.GetLogger(LoggerType.ConsoleLogger);
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request.BypassCache) return await next();

            TResponse response;
            var cachedResponse = await _cachingService.GetAsync<TResponse>(request.CacheKey);
            if (cachedResponse is not null)
            {
                response = cachedResponse;
                await _loggerService.LogResponseWithMessage($"Data brought from Cache -> {request.CacheKey}", request, response);
            }
            else response = await GetResponseAndAddToCache(request, next, cancellationToken);

            return response;
        }

        private async Task<TResponse> GetResponseAndAddToCache(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response = await next();

            await _cachingService.SetAsync(request.CacheKey, response);

            await _loggerService.LogMessage($"Added to Cache -> {request.CacheKey}", request);

            if (request.CacheGroupKey != null)
                await AddCacheKeyToGroup(request, cancellationToken);

            return response;
        }

        //We are adding caches to their group cache if they have, because if any of the cache group member changes, we want to remove all these cache members
        private async Task AddCacheKeyToGroup(TRequest request, CancellationToken cancellationToken)
        {
            var cacheKeysInGroup = await _cachingService.GetAsync<HashSet<string>>(key: request.CacheGroupKey!, cancellationToken);
            
            if (cacheKeysInGroup != null && !cacheKeysInGroup.Contains(request.CacheKey))
                cacheKeysInGroup.Add(request.CacheKey);
            else
                cacheKeysInGroup = new HashSet<string>(new[] { request.CacheKey });

            await _cachingService.SetAsync(key: request.CacheGroupKey!, cacheKeysInGroup, cancellationToken);
            
            await _loggerService.LogMessage($"Added to Cache -> {request.CacheGroupKey}", request);
        }
    }
}