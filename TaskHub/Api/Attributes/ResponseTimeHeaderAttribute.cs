using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Attributes;


public class ResponseTimeHeaderAttribute : ActionFilterAttribute
{
    private readonly Stopwatch stopwatch = new();

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        stopwatch.Start();
        base.OnActionExecuting(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        stopwatch.Stop();
        context.HttpContext.Response.Headers.Append("X-Response-Time-Ms", stopwatch.ElapsedMilliseconds.ToString());
        base.OnActionExecuted(context);
    }
}