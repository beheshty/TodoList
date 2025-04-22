using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Repositories.TodoItems;

namespace TodoList.Application.Commands.TodoItems;

public class UpdateTodoItemCommandHandler : ICommandHandler<UpdateTodoItemCommand, TodoItem>
{
    private readonly ITodoItemRepository _todoItemRepository;

    public UpdateTodoItemCommandHandler(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task<TodoItem> Handle(UpdateTodoItemCommand command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var todoItem = await _todoItemRepository.GetAsync(command.Id, cancellationToken);
        if (todoItem == null)
        {
            throw new KeyNotFoundException($"TodoItem with ID {command.Id} not found.");
        }

        todoItem.Title = command.Title;
        todoItem.Description = command.Description;
        todoItem.DueDate = command.DueDate;

        await _todoItemRepository.UpdateAsync(todoItem, true, cancellationToken);

        return todoItem;
    }
} 