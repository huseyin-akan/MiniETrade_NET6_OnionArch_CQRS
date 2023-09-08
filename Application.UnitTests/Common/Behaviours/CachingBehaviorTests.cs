using MiniETrade.Application.Common.Abstractions;
using MiniETrade.Application.Common.Abstractions.Caching;
using MiniETrade.Application.Common.Abstractions.Logging;
using MiniETrade.Application.Common.Behaviours;
using MiniETrade.Infrastructure.Services.Logging.Loggers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILoggerFactory = MiniETrade.Application.Common.Abstractions.Logging.ILoggerFactory;
using Times = Moq.Times;

namespace Application.UnitTests.Common.Behaviours;

public class CachingBehaviorTests
{
    private readonly CachingBehavior<CachingTestRequest, CachingTestResponse> _cachingBehavior;

    private readonly Mock<ICachingService> _cachingServiceMock; 
    private readonly Mock<ILoggerFactory> _loggerFactoryMock;
    private readonly Mock<ILoggerService> _loggerService;

    public CachingBehaviorTests()
    {
        _cachingServiceMock = new();
        _loggerFactoryMock = new();
        _loggerService = new();

        _loggerFactoryMock.Setup(x => x.GetLogger(It.IsAny<LoggerType>())).Returns(_loggerService.Object);
        _cachingBehavior = new CachingBehavior<CachingTestRequest, CachingTestResponse>(_cachingServiceMock.Object, _loggerFactoryMock.Object);
    }

    [Fact]
    public async Task CachingServiceShouldNotExecuteIfBypassIsTrue()
    {
        //Arrange
        var request = new CachingTestRequest { BypassCache = true};

        //Act
        var actual = await _cachingBehavior.Handle(request, CancellationToken.None, () => Task.FromResult(new CachingTestResponse() ) );

        //Arrange
        actual.Should().NotBeNull();
        _cachingServiceMock.Verify(x => x.GetAsync<CachingTestResponse>(It.IsAny<string>(), CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task ResponseShouldNotBeNullIfBypassCacheIsFalse()
    {
        //Arrange
        var request = new CachingTestRequest { BypassCache = false };

        //Act
        var actual = await _cachingBehavior.Handle(request, CancellationToken.None, () => Task.FromResult(new CachingTestResponse()));

        //Arrange
        actual.Should().NotBeNull();
        _cachingServiceMock.Verify(x => x.GetAsync<CachingTestResponse>(It.IsAny<string>(), CancellationToken.None), Times.Once() );
        _cachingServiceMock.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<CachingTestResponse>(), CancellationToken.None), Times.Once() );
    }

    //TODO-HUS write group-cache tests also. 
}

record CachingTestRequest : IRequest<CachingTestResponse>, ICachableRequest
{
    public string CacheKey { get; set; }

    public bool BypassCache { get; set; }

    public string? CacheGroupKey { get; set; }
}

record CachingTestResponse : IRequestResponse;
