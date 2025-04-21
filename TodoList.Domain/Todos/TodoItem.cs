using TodoList.Domain.Common.Auditing;

namespace TodoList.Domain.Todos;

public class TodoItem : FullAuditedAggregateRoot<Guid>
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public TodoItemStatus Status { get; set; }
    public DateTime? DueDate { get; set; }

    protected TodoItem()
    {
    }

    public TodoItem(Guid id, string title, string? description = null, DateTime? dueDate = null)
        : base(id)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        Status = TodoItemStatus.NotStarted;
    }
} 