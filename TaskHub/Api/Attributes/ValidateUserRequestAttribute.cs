using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace Api.Attributes;

public class ValidateUserRequestAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var parameter = context.ActionArguments.Values.FirstOrDefault();

        if (parameter == null)
        {
            context.Result = new BadRequestObjectResult("Тело запроса отсутствует");
            return;
        }

        var nameProperty = parameter.GetType().GetProperty("Name", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (nameProperty == null)
        {
            base.OnActionExecuting(context);
            return;
        }

        var nameValue = nameProperty.GetValue(parameter) as string;

        if (string.IsNullOrWhiteSpace(nameValue))
        {
            context.Result = new BadRequestObjectResult("Имя пользователя не задано");
            return;
        }

        base.OnActionExecuting(context);
    }
}
