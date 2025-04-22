using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Repositories.TodoItems;

namespace TodoList.Application.Commands.TodoItems;

public class CreateTodoItemCommandHandler : ICommandHandler<CreateTodoItemCommand, TodoItem>
{
    private readonly ITodoItemRepository _todoItemRepository;

    public CreateTodoItemCommandHandler(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task<TodoItem> Handle(CreateTodoItemCommand command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var todoItem = new TodoItem(
            command.Title,
            command.Description,
            command.DueDate);

        await _todoItemRepository.InsertAsync(todoItem, true, cancellationToken);
        return todoItem;
    }
}