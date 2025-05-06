using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Repositories.TodoItems;

namespace TodoList.Application.Queries.TodoItems;

public class GetTodoItemByIdQueryHandler(ITodoItemRepository todoItemRepository)
    : IQueryHandler<GetTodoItemByIdQuery, TodoItem>
{
    public async Task<TodoItem> Handle(GetTodoItemByIdQuery query, CancellationToken cancellationToken = default)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        return await todoItemRepository.GetAsync(query.Id, cancellationToken);
    }
} 