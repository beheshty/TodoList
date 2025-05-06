using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Repositories.TodoItems;

namespace TodoList.Application.Commands.TodoItems;

public class DeleteTodoItemCommandHandler(ITodoItemRepository todoItemRepository)
    : ICommandHandler<DeleteTodoItemCommand, TodoItem>
{
    public async Task<TodoItem> Handle(DeleteTodoItemCommand command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var todoItem = await todoItemRepository.GetAsync(command.Id, cancellationToken);
        
        if (todoItem == null)
            throw new KeyNotFoundException($"TodoItem with ID {command.Id} not found.");

        if (todoItem.Status == TodoItemStatus.Completed)
            throw new InvalidOperationException("Cannot delete a TodoItem that is already completed.");

        await todoItemRepository.DeleteAsync(todoItem, true, cancellationToken);
        
        return todoItem;
    }
} 