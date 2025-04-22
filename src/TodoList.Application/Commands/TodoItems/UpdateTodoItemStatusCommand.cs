using TodoList.Domain.Entities.TodoItems;

namespace TodoList.Application.Commands.TodoItems;

public class UpdateTodoItemStatusCommand : ICommand<TodoItem>
{
    public Guid Id { get; }
    public TodoItemStatus Status { get; }

    public UpdateTodoItemStatusCommand(Guid id, TodoItemStatus status)
    {
        Id = id;
        Status = status;
    }
} 