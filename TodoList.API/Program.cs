using Microsoft.EntityFrameworkCore;
using TodoList.Application.Commands.TodoItems;
using TodoList.Infrastructure.Data;
using TodoList.Infrastructure.Extensions;
using TodoList.Infrastructure.Mediator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Infrastructure Services
builder.Services.AddTodoListDbContext(builder.Configuration.GetConnectionString("DefaultConnection")!);
builder.Services.AddRepositories();

// Register MediatR
builder.Services.AddMediator(typeof(CreateTodoItemCommand).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
