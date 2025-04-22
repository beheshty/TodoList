using System.ComponentModel.DataAnnotations;
using TodoList.Domain.Entities.TodoItems;

namespace TodoList.API.Models;

public class UpdateTodoItemStatusRequest
{
    [Required(ErrorMessage = "Status is required")]
    public TodoItemStatus Status { get; set; }
} 