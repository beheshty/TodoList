using TodoList.Domain.Entities.TodoItems;

namespace TodoList.Application.Queries.TodoItems;

public class GetTodoItemsQuery : IQuery<List<TodoItem>>
{
    public string? SearchTerm { get; set; }
    public TodoItemStatus? Status { get; set; }
    public DateTime? FromDueDate { get; set; }
    public DateTime? ToDueDate { get; set; }
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = 10;
} 