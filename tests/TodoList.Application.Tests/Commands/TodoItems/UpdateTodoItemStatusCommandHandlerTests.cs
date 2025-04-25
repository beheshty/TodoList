using FluentAssertions;
using Moq;
using TodoList.Application.Commands.TodoItems;
using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Repositories.TodoItems;

namespace TodoList.Application.Tests.Commands.TodoItems;

public class UpdateTodoItemStatusCommandHandlerTests
{
    private readonly UpdateTodoItemStatusCommandHandler _handler;
    private readonly Mock<ITodoItemRepository> _todoItemRepositoryMock;

    public UpdateTodoItemStatusCommandHandlerTests()
    {
        _todoItemRepositoryMock = new Mock<ITodoItemRepository>();
        _handler = new UpdateTodoItemStatusCommandHandler(_todoItemRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdateTodoItemStatus()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        var existingTodoItem = new TodoItem("Test Todo", "Test Description");
        var command = new UpdateTodoItemStatusCommand()
        {
            Id = todoId,
            Status = TodoItemStatus.Completed
        };

        _todoItemRepositoryMock
            .Setup(x => x.GetAsync(It.Is<Guid>(id => id == todoId), It.Is<CancellationToken>(ct => ct == CancellationToken.None)))
            .ReturnsAsync(existingTodoItem);

        _todoItemRepositoryMock
            .Setup(x => x.UpdateAsync(
                It.Is<TodoItem>(item => item == existingTodoItem && item.Status == TodoItemStatus.Completed),
                It.Is<bool>(saveChanges => saveChanges == true),
                It.Is<CancellationToken>(ct => ct == CancellationToken.None)))
            .ReturnsAsync((TodoItem item, bool _, CancellationToken _) => item);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(TodoItemStatus.Completed);

        _todoItemRepositoryMock.Verify(
            x => x.GetAsync(It.Is<Guid>(id => id == todoId), It.Is<CancellationToken>(ct => ct == CancellationToken.None)),
            Times.Once);

        _todoItemRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.Is<TodoItem>(item => item == existingTodoItem && item.Status == TodoItemStatus.Completed),
                It.Is<bool>(saveChanges => saveChanges == true),
                It.Is<CancellationToken>(ct => ct == CancellationToken.None)),
            Times.Once);
    }

    [Fact]
    public async Task Handle_NullCommand_ShouldThrowArgumentNullException()
    {
        // Act
        var act = () => _handler.Handle(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
        _todoItemRepositoryMock.Verify(
            x => x.GetAsync(It.Is<Guid>(id => id != Guid.Empty), It.Is<CancellationToken>(ct => ct == CancellationToken.None)),
            Times.Never);
    }

    [Fact]
    public async Task Handle_NonExistentTodoItem_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        var command = new UpdateTodoItemStatusCommand()
        {
            Id = todoId,
            Status = TodoItemStatus.Completed
        };

        _todoItemRepositoryMock
            .Setup(x => x.GetAsync(It.Is<Guid>(id => id == todoId), It.Is<CancellationToken>(ct => ct == CancellationToken.None)))
            .ReturnsAsync((TodoItem)null!);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"TodoItem with ID {todoId} not found.");

        _todoItemRepositoryMock.Verify(
            x => x.GetAsync(It.Is<Guid>(id => id == todoId), It.Is<CancellationToken>(ct => ct == CancellationToken.None)),
            Times.Once);

        _todoItemRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.Is<TodoItem>(item => item != null),
                It.Is<bool>(saveChanges => saveChanges == true),
                It.Is<CancellationToken>(ct => ct == CancellationToken.None)),
            Times.Never);
    }
} 