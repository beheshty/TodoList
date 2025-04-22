using System.ComponentModel.DataAnnotations;
using TodoList.API.Models.Validation;

namespace TodoList.API.Models;

public class CreateTodoItemRequest
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
    [MinLength(3, ErrorMessage = "Title must be at least 3 characters long")]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters")]
    public string? Description { get; set; }
    
    [DataType(DataType.DateTime)]
    [FutureDate]
    public DateTime? DueDate { get; set; }
} 