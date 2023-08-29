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
}