using MediatR;
using MiniETrade.Application.Common.Abstractions;
using MiniETrade.Application.Common.Abstractions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure.Services.Logging.Loggers
{
    public class EFLogger : ILoggerService
    {
        public void Log<TResponse>(Type requestType, IRequest<TResponse> request, TResponse response)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.GetUserId ?? string.Empty;
            string? userName = string.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                userName = await _identityService.GetUserNameAsync(userId);
            }

            _logger.LogInformation("CleanArchitecture Request: {Name} {@UserId} {@UserName} {@Request}",
                requestName, userId, userName, request);
        }

        public void LogException<TResponse>(Type requestType, IRequest<TResponse> request, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
