using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Attributes;

public class FromRouteTaskIdAttribute : Attribute, IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var routeValue = bindingContext.HttpContext.Request.RouteValues["id"]?.ToString();

        if (string.IsNullOrEmpty(routeValue))
        {
            bindingContext.ModelState.AddModelError("id", "Идентификатор задачи не задан");
            bindingContext.Result = ModelBindingResult.Failed();
            return;
        }

        if (!Guid.TryParse(routeValue, out var guidValue))
        {
            bindingContext.ModelState.AddModelError("id", "Идентификатор задачи имеет некорректный формат");
            bindingContext.Result = ModelBindingResult.Failed();
            return;
        }

        bindingContext.Result = ModelBindingResult.Success(guidValue);
        await Task.CompletedTask;
    }
}