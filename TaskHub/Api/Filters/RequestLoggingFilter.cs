using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

/// <summary>
/// Фильтр для логирования запросов
/// </summary>
public class RequestLoggingFilter : IAsyncActionFilter
{
    private readonly ILogger<RequestLoggingFilter> _logger;

    public RequestLoggingFilter(ILogger<RequestLoggingFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpMethod = context.HttpContext.Request.Method;
        var path = context.HttpContext.Request.Path;

        _logger.LogInformation("Начало выполнения: {Method} {Path}", httpMethod, path);

        var stopwatch = Stopwatch.StartNew();
        var executedContext = await next();
        stopwatch.Stop();

        var statusCode = executedContext.HttpContext.Response.StatusCode;
        _logger.LogInformation("Завершение: {Method} {Path} -> {StatusCode} за {Elapsed} мс",
            httpMethod, path, statusCode, stopwatch.ElapsedMilliseconds);
    }
}