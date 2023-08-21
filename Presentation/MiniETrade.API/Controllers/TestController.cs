using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniETrade.Application.Common.Abstractions.Caching;
using MiniETrade.Application.Features.Products.Queries;

namespace MiniETrade.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICachingService _cachingService;
        private readonly IDistibutedCachingService _distibutedCacheService;
        private readonly IInMemoryCachingService _inMemoryCacheService;
        public TestController(ICachingService cachingService, IDistibutedCachingService distibutedCacheService, IInMemoryCachingService inMemoryCacheService)
        {
            _cachingService = cachingService;
            _distibutedCacheService = distibutedCacheService;
            _inMemoryCacheService = inMemoryCacheService;
        }

        [HttpGet("husotest")]
        public IActionResult AddCaching()
        {
            _distibutedCacheService.SetString("huso1", "husokanuslaşmaktır.");
            _distibutedCacheService.Set("husoObj1", new HusoRecord("record1", 31, DateTime.Now));
            _inMemoryCacheService.Set("huso2", "husokanuslaşmaktır.2");
            _inMemoryCacheService.Set("husoObj2", new HusoRecord("record2", 32, DateTime.Now));
            return Ok();
        }

        [HttpGet("husogettest")]
        public IActionResult GetCaching()
        {
            var result1 = _distibutedCacheService.GetString("huso1");
            var result2 = _distibutedCacheService.Get<HusoRecord>("husoObj1");
            var result3 = _distibutedCacheService.Get<string>("huso2");
            var result4 = _distibutedCacheService.Get<HusoRecord>("husoObj2");

            return Ok((result1, result2, result3, result4));
        }
    }
}

public record HusoRecord(string Name, int Age, DateTime Date);
