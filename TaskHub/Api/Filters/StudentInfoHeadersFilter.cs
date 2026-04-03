using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

/// <summary>
/// Фильтр, добавляющий в ответ заголовки с данными студента
/// </summary>
public class StudentInfoHeadersFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        context.HttpContext.Response.Headers.Append("X-Student-Name", "Khrushcheva Victoria Alekseevna");
        context.HttpContext.Response.Headers.Append("X-Student-Group", "RI-240933");

        await next();
    }
}