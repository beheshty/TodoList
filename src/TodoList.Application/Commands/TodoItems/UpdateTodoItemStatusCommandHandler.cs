using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Repositories.TodoItems;

namespace TodoList.Application.Commands.TodoItems;

public class UpdateTodoItemStatusCommandHandler(ITodoItemRepository todoItemRepository)
    : ICommandHandler<UpdateTodoItemStatusCommand, TodoItem>
{
    public async Task<TodoItem> Handle(UpdateTodoItemStatusCommand command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var todoItem = await todoItemRepository.GetAsync(command.Id, cancellationToken);
        
        if (todoItem == null)
            throw new KeyNotFoundException($"TodoItem with ID {command.Id} not found.");

        todoItem.ChangeStatus(command.Status);
        await todoItemRepository.UpdateAsync(todoItem, true, cancellationToken);
        
        return todoItem;
    }
} 