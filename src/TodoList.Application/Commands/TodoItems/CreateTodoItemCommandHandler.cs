using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Repositories.TodoItems;

namespace TodoList.Application.Commands.TodoItems;

public class CreateTodoItemCommandHandler(ITodoItemRepository todoItemRepository)
    : ICommandHandler<CreateTodoItemCommand, TodoItem>
{
    public async Task<TodoItem> Handle(CreateTodoItemCommand command, CancellationToken cancellationToken = default)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var todoItem = new TodoItem(
            command.Title,
            command.Description,
            command.DueDate);

        await todoItemRepository.InsertAsync(todoItem, true, cancellationToken);
        return todoItem;
    }
}