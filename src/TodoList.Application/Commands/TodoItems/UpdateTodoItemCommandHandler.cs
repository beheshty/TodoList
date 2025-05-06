using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Repositories.TodoItems;

namespace TodoList.Application.Commands.TodoItems;

public class UpdateTodoItemCommandHandler(ITodoItemRepository todoItemRepository)
    : ICommandHandler<UpdateTodoItemCommand, TodoItem>
{
    public async Task<TodoItem> Handle(UpdateTodoItemCommand command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var todoItem = await todoItemRepository.GetAsync(command.Id, cancellationToken);
        if (todoItem == null)
        {
            throw new KeyNotFoundException($"TodoItem with ID {command.Id} not found.");
        }

        todoItem.Title = command.Title;
        todoItem.Description = command.Description;
        todoItem.DueDate = command.DueDate;

        await todoItemRepository.UpdateAsync(todoItem, true, cancellationToken);

        return todoItem;
    }
} 