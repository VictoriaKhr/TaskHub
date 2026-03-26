namespace Api.Controllers.Tasks.Request;

/// <summary>Запрос на изменение названия</summary>
public sealed class SetTaskTitleRequest
{
    public string? Title { get; set; }
}