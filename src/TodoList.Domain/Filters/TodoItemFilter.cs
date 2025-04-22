using TodoList.Domain.Entities.TodoItems;

namespace TodoList.Domain.Filters;

public record TodoItemFilter(
    string? SearchTerm = null,
    TodoItemStatus? Status = null,
    DateTime? FromDueDate = null,
    DateTime? ToDueDate = null,
    int SkipCount = 0,
    int MaxResultCount = 10); 