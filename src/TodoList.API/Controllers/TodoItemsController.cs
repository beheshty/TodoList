using Microsoft.AspNetCore.Mvc;
using TodoList.Application.Commands.TodoItems;
using TodoList.Application.Queries.TodoItems;
using TodoList.Application;
using TodoList.API.Models;
using TodoList.API.Mappers;

namespace TodoList.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoItemsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TodoItemsController> _logger;

    public TodoItemsController(IMediator mediator, ILogger<TodoItemsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]

    public async Task<IActionResult> Create([FromBody] CreateTodoItemRequest request)
    {
        _logger.LogInformation("Creating new todo item: {Title}", request.Title);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state when creating todo item");
            return BadRequest(ModelState);
        }

        var command = request.ToCommand();

        var result = await _mediator.Send(command);
        _logger.LogInformation("Successfully created todo item with ID: {Id}", result.Id);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        _logger.LogInformation("Getting todo item by ID: {Id}", id);

        var query = new GetTodoItemByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            _logger.LogWarning("Todo item not found: {Id}", id);
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] GetTodoItemsRequest request)
    {
        _logger.LogInformation("Getting todo items list");

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state when getting todo items list");
            return BadRequest(ModelState);
        }

        var query = request.ToQuery();

        var result = await _mediator.Send(query);
        _logger.LogInformation("Retrieved todo items");
        return Ok(result);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateTodoItemStatusRequest request)
    {
        _logger.LogInformation("Updating status for todo item: {Id}", id);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state when updating todo item status");
            return BadRequest(ModelState);
        }

        var command = request.ToCommand(id);

        await _mediator.Send(command);
        _logger.LogInformation("Successfully updated status for todo item: {Id}", id);
        return NoContent();
    }


    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTodoItemRequest request)
    {
        _logger.LogInformation("Updating todo item: {Id}", id);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state when updating todo item");
            return BadRequest(ModelState);
        }

        var command = request.ToCommand(id);

        var result = await _mediator.Send(command);
        _logger.LogInformation("Successfully updated todo item: {Id}", id);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation("Deleting todo item: {Id}", id);

        var command = new DeleteTodoItemCommand { Id = id };
        var result = await _mediator.Send(command);
        _logger.LogInformation("Successfully deleted todo item: {Id}", id);
        return Ok(result);
    }
}