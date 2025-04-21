using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Entities.TodoItems;

namespace TodoList.Infrastructure.Data;

public class TodoListDbContext : DbContext
{
    public DbSet<TodoItem> TodoItems { get; set; }

    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoListDbContext).Assembly);
    }
} 