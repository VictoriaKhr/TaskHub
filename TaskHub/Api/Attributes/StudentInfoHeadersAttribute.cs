using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Attributes;

public class StudentInfoHeadersAttribute : ResultFilterAttribute
{
    private readonly string _studentName = "Khrushcheva Victoria Alekseevna";
    private readonly string _studentGroup = "RI-240933";

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        context.HttpContext.Response.Headers.TryAdd(
            "X-Student-Name",
            _studentName
        );

        context.HttpContext.Response.Headers.TryAdd(
            "X-Student-Group",
            _studentGroup
        );

        base.OnResultExecuting(context);
    }
}