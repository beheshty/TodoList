using Microsoft.AspNetCore.Mvc;
using TodoList.Application.Commands.TodoItems;
using TodoList.Application.Queries.TodoItems;
using TodoList.Application;
using TodoList.API.Models;
using TodoList.API.Mappers;

namespace TodoList.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoItemsController(IMediator mediator, ILogger<TodoItemsController> logger) : ControllerBase
{
    [HttpPost]

    public async Task<IActionResult> Create([FromBody] CreateTodoItemRequest request)
    {
        logger.LogInformation("Creating new todo item: {Title}", request.Title);

        if (!ModelState.IsValid)
        {
            logger.LogWarning("Invalid model state when creating todo item");
            return BadRequest(ModelState);
        }

        var command = request.ToCommand();

        var result = await mediator.Send(command);
        logger.LogInformation("Successfully created todo item with ID: {Id}", result.Id);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        logger.LogInformation("Getting todo item by ID: {Id}", id);

        var query = new GetTodoItemByIdQuery { Id = id };
        var result = await mediator.Send(query);
        
        if (result.Status != 0) return Ok(result);
        
        logger.LogWarning("Todo item not found with ID: {Id}", id);
        return NotFound();

    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] GetTodoItemsRequest request)
    {
        logger.LogInformation("Getting todo items list");

        if (!ModelState.IsValid)
        {
            logger.LogWarning("Invalid model state when getting todo items list");
            return BadRequest(ModelState);
        }

        var query = request.ToQuery();

        var result = await mediator.Send(query);
        logger.LogInformation("Retrieved todo items");
        return Ok(result);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateTodoItemStatusRequest request)
    {
        logger.LogInformation("Updating status for todo item: {Id}", id);

        if (!ModelState.IsValid)
        {
            logger.LogWarning("Invalid model state when updating todo item status");
            return BadRequest(ModelState);
        }

        var command = request.ToCommand(id);

        await mediator.Send(command);
        logger.LogInformation("Successfully updated status for todo item: {Id}", id);
        return NoContent();
    }


    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTodoItemRequest request)
    {
        logger.LogInformation("Updating todo item: {Id}", id);

        if (!ModelState.IsValid)
        {
            logger.LogWarning("Invalid model state when updating todo item");
            return BadRequest(ModelState);
        }

        var command = request.ToCommand(id);

        var result = await mediator.Send(command);
        logger.LogInformation("Successfully updated todo item: {Id}", id);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        logger.LogInformation("Deleting todo item: {Id}", id);

        var command = new DeleteTodoItemCommand { Id = id };
        var result = await mediator.Send(command);
        logger.LogInformation("Successfully deleted todo item: {Id}", id);
        return Ok(result);
    }
}