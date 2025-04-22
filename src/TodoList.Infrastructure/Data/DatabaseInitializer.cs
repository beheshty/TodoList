using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TodoList.Infrastructure.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(TodoListDbContext context, ILogger logger)
    {
        try
        {
            // Apply migrations and create database if it doesn't exist
            await context.Database.MigrateAsync();
            
            logger.LogInformation("Database initialized successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }
} 