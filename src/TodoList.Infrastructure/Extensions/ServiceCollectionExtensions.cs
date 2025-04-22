using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TodoList.Infrastructure.Data;

namespace TodoList.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<TodoListDbContext>()
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }

    public static IServiceCollection AddTodoListDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TodoListDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }

    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<TodoListDbContext>();
        var logger = services.GetRequiredService<ILogger<TodoListDbContext>>();

        await DatabaseInitializer.InitializeAsync(context, logger);
    }
} 