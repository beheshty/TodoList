
using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Repositories.TodoItems;
using TodoList.Infrastructure.Data;
using TodoList.Infrastructure.Repositories.Base;

namespace TodoList.Infrastructure.Repositories
{
    public class TodoItemRepository : EfRepository<TodoItem, Guid, TodoListDbContext>, ITodoItemRepository
    {
        public TodoItemRepository(TodoListDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<TodoItem>> GetByStatusAsync(TodoItemStatus status, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<TodoItem>> GetOverdueAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
