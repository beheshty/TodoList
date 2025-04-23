using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Repositories.TodoItems;

namespace TodoList.Application.Commands.TodoItems;

public class DeleteTodoItemCommandHandler : ICommandHandler<DeleteTodoItemCommand, TodoItem>
{
    private readonly ITodoItemRepository _todoItemRepository;

    public DeleteTodoItemCommandHandler(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task<TodoItem> Handle(DeleteTodoItemCommand command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var todoItem = await _todoItemRepository.GetAsync(command.Id, cancellationToken);
        
        if (todoItem == null)
            throw new KeyNotFoundException($"TodoItem with ID {command.Id} not found.");

        if (todoItem.Status == TodoItemStatus.Completed)
            throw new InvalidOperationException("Cannot delete a TodoItem that is already completed.");

        await _todoItemRepository.DeleteAsync(todoItem, true, cancellationToken);
        
        return todoItem;
    }
} 