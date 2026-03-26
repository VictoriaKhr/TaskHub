using Dal.Repositories;
using Dal.Repositories.Interfaces;
using Logic.Tasks.Models;
using Logic.Tasks.Services.Interfaces;

namespace Logic.Tasks.Services;

/// <summary>Реализация сервиса задач</summary>

/// <inheritdoc />
public sealed class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    /// <inheritdoc />
    public async Task<TaskModel> CreateTaskAsync(string title, Guid userId, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.CreateAsync(title, userId, cancellationToken);
        return new TaskModel(task.Id, task.Title ?? string.Empty, task.CreatedByUserId, task.CreatedUtc);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<TaskModel>> GetAllTasksAsync(CancellationToken cancellationToken)
    {
        var allTasks = await _taskRepository.GetAllAsync(cancellationToken);

        var taskModels = allTasks
            .Select(t => new TaskModel(t.Id, t.Title ?? string.Empty, t.CreatedByUserId, t.CreatedUtc))
            .ToList()
            .AsReadOnly();

        return taskModels;
    }

    /// <inheritdoc />
    public async Task<TaskModel?> GetTaskByIdAsync(Guid taskId, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken);
        return task is null
            ? null
            : new TaskModel(task.Id, task.Title ?? string.Empty, task.CreatedByUserId, task.CreatedUtc);
    }

    /// <inheritdoc />
    public async Task SetTaskTitleAsync(Guid taskId, string title, CancellationToken cancellationToken)
    {
        await _taskRepository.UpdateTitleAsync(taskId, title, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteTaskByIdAsync(Guid taskId, CancellationToken cancellationToken)
    {
        return await _taskRepository.DeleteByIdAsync(taskId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DeleteAllTasksAsync(CancellationToken cancellationToken)
    {
        await _taskRepository.DeleteAllAsync(cancellationToken);
    }
}