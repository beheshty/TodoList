using Riok.Mapperly.Abstractions;
using TodoList.API.Models;
using TodoList.Application.Commands.TodoItems;
using TodoList.Application.Queries.TodoItems;

namespace TodoList.API.Mappers;

[Mapper]
public static partial class TodoItemMapper
{
    public static partial CreateTodoItemCommand ToCommand(this CreateTodoItemRequest request);

    public static partial UpdateTodoItemCommand ToCommand(this UpdateTodoItemRequest request, Guid id);

    public static partial UpdateTodoItemStatusCommand ToCommand(this UpdateTodoItemStatusRequest request, Guid id);

    public static partial GetTodoItemsQuery ToQuery(this GetTodoItemsRequest request);
} 