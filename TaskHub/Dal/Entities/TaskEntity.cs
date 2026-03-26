using System.ComponentModel.DataAnnotations.Schema;

namespace Dal.Entities;

/// <summary>Задача пользователя</summary>
[Table("tasks")]
public class TaskEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid CreatedByUserId { get; set; }
    public DateTimeOffset CreatedUtc { get; set; }

    public User? User { get; set; }
}