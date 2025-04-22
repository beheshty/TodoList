using System.ComponentModel.DataAnnotations;
using TodoList.Domain.Entities.TodoItems;

namespace TodoList.API.Models;

public class GetTodoItemsRequest
{
    public string? SearchTerm { get; set; }
    
    public TodoItemStatus? Status { get; set; }
    
    public DateTime? FromDueDate { get; set; }
    
    public DateTime? ToDueDate { get; set; }
    
    [Range(0, int.MaxValue, ErrorMessage = "SkipCount must be a non-negative number")]
    public int SkipCount { get; set; } = 0;
    
    [Range(1, 100, ErrorMessage = "MaxResultCount must be between 1 and 100")]
    public int MaxResultCount { get; set; } = 10;
} 