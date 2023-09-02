using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using MiniETrade.Application.Common.Abstractions;
using MiniETrade.Application.Common.Abstractions.Caching;
using MiniETrade.Application.Common.Abstractions.Identity;
using MiniETrade.Application.Common.Abstractions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILoggerFactory = MiniETrade.Application.Common.Abstractions.Logging.ILoggerFactory;

namespace MiniETrade.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>, ILoggableRequest 
    {
        //TODO-HUS ayrı bir konu da, çoklu dil sistemi ile ilgili, messages içerisinde English, Russian ve Turkish desteği getirelim.

        //TODO-HUS tüm istekleri loglayabilecek bir yapı kurabiliriz. Ya da istersek sadece interface'i implement eden requestleri loglarız. Ve tüm isterkleri loglarken [NotLogged] attribute'üne sahip ya da, INotLoggedRequest immplement eden loglanmaz gibi de yapabiliriz. Hatta yapalım.
        private readonly ILoggerService _loggerService;

        public LoggingBehaviour(ILoggerFactory loggerFactory)
        {
            _loggerService = loggerFactory.CreateLogger(LoggerType.EFLogger);
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();
            _loggerService.Log(typeof(TRequest), request, response);

            return response;
        }
    }
}