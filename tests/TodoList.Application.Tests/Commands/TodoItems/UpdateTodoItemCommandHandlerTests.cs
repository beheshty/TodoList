using FluentAssertions;
using Moq;
using TodoList.Application.Commands.TodoItems;
using TodoList.Application.Tests.Common;
using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Repositories.TodoItems;

namespace TodoList.Application.Tests.Commands.TodoItems;

public class UpdateTodoItemCommandHandlerTests : TestBase
{
    private readonly UpdateTodoItemCommandHandler _handler;
    private readonly Mock<ITodoItemRepository> _todoItemRepositoryMock;

    public UpdateTodoItemCommandHandlerTests()
    {
        _todoItemRepositoryMock = new Mock<ITodoItemRepository>();
        _handler = new UpdateTodoItemCommandHandler(_todoItemRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdateTodoItem()
    {
        // Arrange
        var existingTodoItem = new TodoItem("Original Title", "Original Description")
        {
            Id = Guid.NewGuid(),
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        var command = new UpdateTodoItemCommand()
        {
            Id = existingTodoItem.Id,
            Title = "Updated Title",
            Description = "Updated Description",
            DueDate = DateTime.UtcNow.AddDays(2)
        };

        _todoItemRepositoryMock
            .Setup(x => x.GetAsync(existingTodoItem.Id, It.Is<CancellationToken>(c => c == default)))
            .ReturnsAsync(existingTodoItem);

        _todoItemRepositoryMock
            .Setup(x => x.UpdateAsync(It.Is<TodoItem>(item => 
                item.Id == existingTodoItem.Id &&
                item.Title == command.Title &&
                item.Description == command.Description &&
                item.DueDate == command.DueDate), It.Is<bool>(b => b == true), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTodoItem);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(existingTodoItem.Id);
        result.Title.Should().Be(command.Title);
        result.Description.Should().Be(command.Description);
        result.DueDate.Should().Be(command.DueDate);
        result.Status.Should().Be(existingTodoItem.Status);

        _todoItemRepositoryMock.Verify(
            x => x.GetAsync(existingTodoItem.Id, It.Is<CancellationToken>(c => c == default)),
            Times.Once);

        _todoItemRepositoryMock.Verify(
            x => x.UpdateAsync(It.Is<TodoItem>(item => 
                item.Id == existingTodoItem.Id &&
                item.Title == command.Title &&
                item.Description == command.Description &&
                item.DueDate == command.DueDate), It.Is<bool>(b => b == true), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistentTodoItem_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var command = new UpdateTodoItemCommand()
        {
            Id = Guid.NewGuid(),
            Title = "Updated Title",
            Description = "Updated Description",
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        _todoItemRepositoryMock
            .Setup(x => x.GetAsync(command.Id, It.Is<CancellationToken>(c => c == default)))
            .ReturnsAsync((TodoItem?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"TodoItem with ID {command.Id} not found.");

        _todoItemRepositoryMock.Verify(
            x => x.GetAsync(command.Id, It.Is<CancellationToken>(c => c == default)),
            Times.Once);

        _todoItemRepositoryMock.Verify(
            x => x.UpdateAsync(It.Is<TodoItem>(item => 
                item.Id == command.Id &&
                item.Title == command.Title &&
                item.Description == command.Description &&
                item.DueDate == command.DueDate), It.Is<bool>(b => b == true), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_NullCommand_ShouldThrowArgumentNullException()
    {
        // Act
        var act = () => _handler.Handle(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
        _todoItemRepositoryMock.Verify(
            x => x.GetAsync(It.Is<Guid>(id => id != Guid.Empty), It.Is<CancellationToken>(c => c == default)),
            Times.Never);

        _todoItemRepositoryMock.Verify(
            x => x.UpdateAsync(It.Is<TodoItem>(item => item != null), It.Is<bool>(b => b == true), It.IsAny<CancellationToken>()),
            Times.Never);
    }
} 