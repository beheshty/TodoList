using TodoList.Domain.Entities.TodoItems;

namespace TodoList.Application.Commands.TodoItems;

public class DeleteTodoItemCommand : ICommand<TodoItem>
{
    public Guid Id { get; set; }
} 