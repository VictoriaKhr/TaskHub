using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Api.Controllers.Tasks.Request;

namespace Api.Filters;

/// <summary>
/// Валидация запроса на создание задачи
/// </summary>
public class ValidateCreateTaskRequestFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.ActionArguments["request"] as CreateTaskRequest;

        if (request is null)
        {
            context.Result = new BadRequestObjectResult("Тело запроса отсутствует");
            return;
        }

        if (request.UserId == Guid.Empty)
        {
            context.Result = new BadRequestObjectResult("Идентификатор пользователя не задан");
            return;
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            context.Result = new BadRequestObjectResult("Название задачи не задано");
            return;
        }

        await next();
    }
}