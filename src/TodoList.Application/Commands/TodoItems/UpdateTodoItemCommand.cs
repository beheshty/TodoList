using TodoList.Domain.Entities.TodoItems;

namespace TodoList.Application.Commands.TodoItems;

public class UpdateTodoItemCommand : ICommand<TodoItem>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
} 