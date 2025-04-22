
using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Repositories.TodoItems;
using TodoList.Infrastructure.Data;
using TodoList.Infrastructure.Repositories.Base;

namespace TodoList.Infrastructure.Repositories
{
    public class TodoItemRepository(TodoListDbContext dbContext) : EfRepository<TodoItem, Guid, TodoListDbContext>(dbContext), ITodoItemRepository
    {
        public Task<List<TodoItem>> GetByStatusAsync(TodoItemStatus status, CancellationToken cancellationToken = default)
        {
            return GetDbContext().TodoItems.Where(x => x.Status == status).ToListAsync(cancellationToken);
        }

        public Task<List<TodoItem>> GetListAsync(TodoItemFilter filter, CancellationToken cancellationToken = default)
        {
            var query = GetDbContext().TodoItems.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                query = query.Where(x => x.Title.Contains(filter.SearchTerm) || (!string.IsNullOrEmpty( x.Description) && x.Description.Contains(filter.SearchTerm)));
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(x => x.Status == filter.Status.Value);
            }

            if (filter.FromDueDate.HasValue)
            {
                query = query.Where(x => x.DueDate >= filter.FromDueDate.Value);
            }

            if (filter.ToDueDate.HasValue)
            {
                query = query.Where(x => x.DueDate <= filter.ToDueDate.Value);
            }

            query = query.OrderByDescending(x => x.CreationTime);

            query = query.Skip(filter.SkipCount).Take(filter.MaxResultCount);

            return query.ToListAsync(cancellationToken);
        }

        public Task<List<TodoItem>> GetOverdueAsync(CancellationToken cancellationToken = default)
        {
            return GetDbContext().TodoItems.Where(x => x.DueDate < DateTime.UtcNow).ToListAsync(cancellationToken);
        }
    }
}
