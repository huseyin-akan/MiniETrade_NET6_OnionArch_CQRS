using MediatR;
using MiniETrade.Application.Common.Abstractions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure.Services.Logging.Loggers
{
    public class MongoLogger : ILoggerService
    {
        public void Log<TResponse>(Type requestType, IRequest<TResponse> request, TResponse response)
        {
            throw new NotImplementedException();
        }

        public void LogException<TResponse>(Type requestType, IRequest<TResponse> request, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
