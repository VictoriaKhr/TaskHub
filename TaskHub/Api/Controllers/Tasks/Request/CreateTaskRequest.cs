namespace Api.Controllers.Tasks.Request;

/// <summary>Запрос на создание задачи</summary>
public sealed class CreateTaskRequest
{
    /// <summary>Название задачи</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Идентификатор пользователя, создающего задачу</summary>
    public Guid UserId { get; set; }
}