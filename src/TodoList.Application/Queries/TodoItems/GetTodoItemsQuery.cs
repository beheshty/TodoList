using TodoList.Domain.Entities.TodoItems;

namespace TodoList.Application.Queries.TodoItems;

public record GetTodoItemsQuery(
    string? SearchTerm = null,
    TodoItemStatus? Status = null,
    DateTime? FromDueDate = null,
    DateTime? ToDueDate = null,
    int SkipCount = 0,
    int MaxResultCount = 10) : IQuery<List<TodoItem>>; 