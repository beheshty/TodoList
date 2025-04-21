using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Repositories.TodoItems;

namespace TodoList.Application.Queries.TodoItems;

public class GetTodoItemByIdQueryHandler : IQueryHandler<GetTodoItemByIdQuery, TodoItem>
{
    private readonly ITodoItemRepository _todoItemRepository;

    public GetTodoItemByIdQueryHandler(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task<TodoItem> Handle(GetTodoItemByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _todoItemRepository.GetAsync(query.Id, cancellationToken);
    }
} 