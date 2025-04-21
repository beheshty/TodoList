using TodoList.Domain.Entities.TodoItems;

namespace TodoList.Application.Commands.TodoItems;

public class CreateTodoItemCommand : ICommand<TodoItem>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid TodoListId { get; set; }
}