using System.Linq.Expressions;
using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Filters;
using TodoList.Domain.Repositories.Base;

namespace TodoList.Domain.Repositories.TodoItems;

public interface ITodoItemRepository : IRepository<TodoItem, Guid>
{
    Task<List<TodoItem>> GetByStatusAsync(TodoItemStatus status, CancellationToken cancellationToken = default);
    Task<List<TodoItem>> GetOverdueAsync(CancellationToken cancellationToken = default);
    Task<List<TodoItem>> GetListAsync(TodoItemFilter filter, CancellationToken cancellationToken = default);
}