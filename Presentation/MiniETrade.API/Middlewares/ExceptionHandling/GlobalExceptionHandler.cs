using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MiniETrade.Application.Common.Abstractions.Localization;
using MiniETrade.Domain.Exceptions;
using MiniETrade.Domain.Exceptions.ProblemDetail;
using Newtonsoft.Json;
using System.Net;
using ValidationProblemDetails = MiniETrade.Domain.Exceptions.ProblemDetail.ValidationProblemDetails;

namespace MiniETrade.API.Middlewares.ExceptionHandling;

public class GlobalExceptionHandler : IMiddleware 
{
    private readonly RequestDelegate _next;
    private readonly ILanguageService _languageService;

    public GlobalExceptionHandler(RequestDelegate next, ILanguageService languageService)
    {
        _next = next;
        _languageService = languageService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            context.Response.ContentType = "application/json";

            _ = exception switch
            {
                BusinessException businessException => HandleException(context, businessException),
                ValidationException validationException => HandleException(context, validationException),
                _ => HandleException(context)
            };
        }
    }

    private static Task HandleException(HttpContext context, BusinessException exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        var problemDetail = new BusinessProblemDetails(exception.Message).AsJson();
        return context.Response.WriteAsync(problemDetail);
    }

    private static Task HandleException(HttpContext context, ValidationException exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        var problemDetail = new ValidationProblemDetails(exception.Errors).AsJson();
        return context.Response.WriteAsync(problemDetail);
    }

    private static Task HandleException(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var unknownProblemDetail = new InternalServerErrorProblemDetails().AsJson();
        return context.Response.WriteAsync(unknownProblemDetail);
    }

    private string HandleMessageLanguages(string key)
    {
        //TODO-HUS eğer key varsa ilgili dile çevirip hata fırlatıcaz. Eğer key yok ise, gelen stringi fırlatıcaz.
        //TODO-HUS ama düşünelim hiç key olmayan bir durum olmaması gerek sanki. Belki bunu sağlamak için BusinessException string değil de Messages'ın bir öğresini almak zorunda bırakılabilir.
        return null;
    }
}