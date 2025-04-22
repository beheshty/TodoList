using Microsoft.EntityFrameworkCore;
using TodoList.Application.Commands.TodoItems;
using TodoList.Infrastructure.Extensions;
using TodoList.Infrastructure.Mediator;
using TodoList.Infrastructure.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Infrastructure Services
builder.Services.AddTodoListDbContext(builder.Configuration.GetConnectionString("DefaultConnection")!);
builder.Services.AddRepositories();

// Add MediatR
builder.Services.AddMediator(typeof(CreateTodoItemCommand).Assembly);

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<TodoListDbContext>("database");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Initialize Database
await app.Services.InitializeDatabaseAsync();

app.UseHttpsRedirection();

// Add health check endpoint
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        };

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(result);
    }
});

app.MapControllers();

app.Run();
