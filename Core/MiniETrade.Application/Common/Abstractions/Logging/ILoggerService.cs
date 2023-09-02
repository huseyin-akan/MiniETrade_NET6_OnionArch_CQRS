using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions.Logging
{
    public interface ILoggerService
    {
        void LogException<TResponse>(Type requestType, IRequest<TResponse> request, Exception exception); //TODO-HUS ilk parametre gereksiz olabilir.
        void Log<TResponse>(Type requestType, IRequest<TResponse> request, TResponse response); //TODO-HUS ilk parametre gereksiz olabilir.
    }
}
