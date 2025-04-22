using TodoList.Domain.Common.Auditing;

namespace TodoList.Domain.Entities.TodoItems;

public class TodoItem : CreationAuditedAggregateRoot<Guid>
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public TodoItemStatus Status { get; private set; }

    protected TodoItem()
    {
    }

    public TodoItem(string title, string? description = null, DateTime? dueDate = null)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description;
        DueDate = dueDate;
        Status = TodoItemStatus.InProgress;
    }

    public void ChangeStatus(TodoItemStatus status)
    {
        Status = status;
    }
} 