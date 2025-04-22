using TodoList.Domain.Entities.TodoItems;

namespace TodoList.Application.Commands.TodoItems;

public class UpdateTodoItemCommand : ICommand<TodoItem>
{
    public Guid Id { get; }
    public string Title { get; }
    public string? Description { get; }
    public DateTime? DueDate { get; }

    public UpdateTodoItemCommand(Guid id, string title, string? description, DateTime? dueDate)
    {
        Id = id;
        Title = title;
        Description = description;
        DueDate = dueDate;
    }
} 