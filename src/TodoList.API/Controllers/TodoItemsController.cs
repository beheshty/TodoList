using Microsoft.AspNetCore.Mvc;
using TodoList.Application.Commands.TodoItems;
using TodoList.Application.Queries.TodoItems;
using TodoList.Application;
using TodoList.Domain.Entities.TodoItems;
using TodoList.API.Models;

namespace TodoList.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoItemsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTodoItemCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetTodoItemByIdQuery(id);
        var result = await mediator.Send(query);
        
        if (result == null)
            return NotFound();
            
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] GetTodoItemsRequest request)
    {
        var query = new GetTodoItemsQuery(
            SearchTerm: request.SearchTerm,
            Status: request.Status,
            FromDueDate: request.FromDueDate,
            ToDueDate: request.ToDueDate,
            SkipCount: request.SkipCount,
            MaxResultCount: request.MaxResultCount);

        var result = await mediator.Send(query);
        return Ok(result);
    }
} 