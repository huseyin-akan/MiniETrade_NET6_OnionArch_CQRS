using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using MiniETrade.Domain.Exceptions;
using MiniETrade.Domain.Exceptions.ProblemDetail;
using Newtonsoft.Json;
using System.Net;
using ValidationProblemDetails = MiniETrade.Domain.Exceptions.ProblemDetail.ValidationProblemDetails;

namespace MiniETrade.API.Middlewares.ExceptionHandling;

public class GlobalExceptionHandler : IMiddleware 
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandler(RequestDelegate next)
    {
        _next = next;
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
                _ => HandleException(context, exception)
            };
        }
    }

    private Task HandleException(HttpContext context, BusinessException exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        var problemDetail = new BusinessProblemDetails(exception.Message).AsJson();
        return context.Response.WriteAsync(problemDetail);
    }

    private Task HandleException(HttpContext context, ValidationException exception)
    {
        object errors = null;

        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        errors = ((ValidationException)exception).Errors;

        return context.Response.WriteAsync(new ValidationProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://example.com/probs/validation",
            Title = "Validation error(s)",
            Detail = (errors as IEnumerable<ValidationFailure>)?.FirstOrDefault()?.ToString(),
            Instance = "",
            Errors = errors
        }.ToString());
    }

    private Task HandleException(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var unknownProblemDetail = new InternalServerErrorProblemDetails().AsJson();
        return context.Response.WriteAsync(unknownProblemDetail);
    }
}