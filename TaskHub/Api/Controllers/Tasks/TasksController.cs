using Api.Attributes;
using Api.Controllers.Tasks.Request;
using Api.Controllers.Tasks.Response;
using Api.UseCases.Tasks.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Tasks;

/// <summary>
/// Контроллер работы с задачами
/// </summary>
[ApiController]
[Route("tasks")]
[ResponseTimeHeader]
[StudentInfoHeaders]
public sealed class TasksController : ControllerBase
{
    /// <summary>
    /// UseCase управления задачами
    /// </summary>
    private readonly IManageTaskUseCase _taskUseCase;
    private static readonly Guid DefaultUserId = Guid.Parse("b92220ab-e99b-4df5-a02e-5d123d877994");

    public TasksController(IManageTaskUseCase taskUseCase)
    {
        _taskUseCase = taskUseCase;
    }

    /// <summary>
    /// Создать задачу
    /// </summary>
    [HttpPost]
    [ValidateUserRequest]
    public async Task<ActionResult<TaskResponse>> CreateTaskAsync(
        [FromBody] CreateTaskRequest? request,
        CancellationToken cancellationToken)
    {
        var userId = DefaultUserId;
        var task = await _taskUseCase.CreateTaskAsync(request!.Title ?? string.Empty, userId, cancellationToken);
        return StatusCode(201, task);
    }

    /// <summary>
    /// Получить все задачи
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<TaskResponse>>> GetAllTasksAsync(CancellationToken cancellationToken)
    {
        var response = await _taskUseCase.GetAllTasksAsync(cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Получить задачу по идентификатору
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskResponse>> GetTaskByIdAsync(
        [ModelBinder(typeof(FromRouteTaskIdAttribute))] Guid id,
        CancellationToken cancellationToken)
    {
        var taskResponse = await _taskUseCase.GetTaskByIdAsync(id, cancellationToken);
        if (taskResponse is null)
        {
            return NotFound();
        }
        return Ok(taskResponse);
    }

    /// <summary>
    /// Изменить название задачи
    /// </summary>
    [HttpPut("{id}/title")]
    [ValidateUserRequest]
    public async Task<IActionResult> SetTaskTitleAsync(
        [ModelBinder(typeof(FromRouteTaskIdAttribute))] Guid id,
        [FromBody] SetTaskTitleRequest? request,
        CancellationToken cancellationToken)
    {
        await _taskUseCase.SetTaskTitleAsync(id, request!.Title ?? string.Empty, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Удалить задачу по идентификатору
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTaskByIdAsync(
        [ModelBinder(typeof(FromRouteTaskIdAttribute))] Guid id,
        CancellationToken cancellationToken)
    {
        var deleted = await _taskUseCase.DeleteTaskByIdAsync(id, cancellationToken);
        if (deleted == false)
        {
            return NotFound();
        }
        return NoContent();
    }

    /// <summary>
    /// Удалить все задачи
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> DeleteAllTasksAsync(CancellationToken cancellationToken)
    {
        await _taskUseCase.DeleteAllTasksAsync(cancellationToken);
        return NoContent();
    }
}