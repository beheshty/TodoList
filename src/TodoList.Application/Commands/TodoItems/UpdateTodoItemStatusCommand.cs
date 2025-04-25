using TodoList.Domain.Entities.TodoItems;

namespace TodoList.Application.Commands.TodoItems;

public class UpdateTodoItemStatusCommand : ICommand<TodoItem>
{
    public Guid Id { get; set; }
    public TodoItemStatus Status { get; set; }
} 