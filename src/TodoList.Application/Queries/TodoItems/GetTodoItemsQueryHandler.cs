using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Filters;
using TodoList.Domain.Repositories.TodoItems;

namespace TodoList.Application.Queries.TodoItems;

public class GetTodoItemsQueryHandler(ITodoItemRepository todoItemRepository)
    : IQueryHandler<GetTodoItemsQuery, List<TodoItem>>
{
    public async Task<List<TodoItem>> Handle(GetTodoItemsQuery query, CancellationToken cancellationToken = default)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        var filter = new TodoItemFilter(
            SearchTerm: query.SearchTerm,
            Status: query.Status,
            FromDueDate: query.FromDueDate,
            ToDueDate: query.ToDueDate,
            SkipCount: query.SkipCount,
            MaxResultCount: query.MaxResultCount);

        return await todoItemRepository.GetListAsync(filter, cancellationToken);
    }
} 