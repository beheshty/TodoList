using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Entities.TodoItems;

namespace TodoList.Infrastructure.Data;

public class TodoListDbContext(DbContextOptions<TodoListDbContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoListDbContext).Assembly);
    }
} 