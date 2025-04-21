
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

        public Task<List<TodoItem>> GetOverdueAsync(CancellationToken cancellationToken = default)
        {
            return GetDbContext().TodoItems.Where(x => x.DueDate < DateTime.UtcNow).ToListAsync(cancellationToken);
        }
    }
}
