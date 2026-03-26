namespace Api.Controllers.Tasks.Request;

/// <summary>Запрос на создание задачи</summary>
public sealed class CreateTaskRequest
{
    public string? Title { get; set; }
}