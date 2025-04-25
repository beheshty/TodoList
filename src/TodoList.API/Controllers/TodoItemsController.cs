using Microsoft.AspNetCore.Mvc;
using TodoList.Application.Commands.TodoItems;
using TodoList.Application.Queries.TodoItems;
using TodoList.Application;
using TodoList.API.Models;
using TodoList.API.Mappers;

namespace TodoList.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoItemsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTodoItemRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var command = request.ToCommand();
        
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetTodoItemByIdQuery { Id = id };
        var result = await mediator.Send(query);
        
        if (result == null)
            return NotFound();
            
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] GetTodoItemsRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var query = request.ToQuery();

        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateTodoItemStatusRequest request)
    {
         if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var command = request.ToCommand(id);
        
        await mediator.Send(command);
        return NoContent();
    }


    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTodoItemRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = request.ToCommand(id);

        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteTodoItemCommand { Id = id };
        var result = await mediator.Send(command);
        return Ok(result);
    }
} 