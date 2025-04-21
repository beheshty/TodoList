using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Domain.Repositories.Base;
using TodoList.Domain.Todos;

namespace TodoList.Domain.Repositories.Todos;

public interface ITodoItemRepository : IRepository<TodoItem, Guid>
{
    Task<List<TodoItem>> GetByStatusAsync(TodoItemStatus status, CancellationToken cancellationToken = default);
    Task<List<TodoItem>> GetOverdueAsync(CancellationToken cancellationToken = default);
} 