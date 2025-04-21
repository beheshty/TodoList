using System.Threading;
using System.Threading.Tasks;
using TodoList.Domain.CQRS.Commands;
using TodoList.Domain.CQRS.Commands.TodoItems;
using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Repositories;

namespace TodoList.Infrastructure.CQRS.Commands.TodoItems;

public class CreateTodoItemCommandHandler : ICommandHandler<CreateTodoItemCommand, TodoItem>
{
    private readonly ITodoItemRepository _todoItemRepository;

    public CreateTodoItemCommandHandler(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task<TodoItem> Handle(CreateTodoItemCommand command, CancellationToken cancellationToken = default)
    {
        var todoItem = new TodoItem(
            command.Title,
            command.Description,
            command.DueDate);

        await _todoItemRepository.InsertAsync(todoItem, cancellationToken);
        return todoItem;
    }
} 