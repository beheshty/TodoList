using FluentAssertions;
using Moq;
using TodoList.Application.Commands.TodoItems;
using TodoList.Application.Tests.Common;
using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Repositories.TodoItems;

namespace TodoList.Application.Tests.Commands.TodoItems;

public class DeleteTodoItemCommandHandlerTests : TestBase
{
    private readonly DeleteTodoItemCommandHandler _handler;
    private readonly Mock<ITodoItemRepository> _todoItemRepositoryMock;

    public DeleteTodoItemCommandHandlerTests()
    {
        _todoItemRepositoryMock = new Mock<ITodoItemRepository>();
        _handler = new DeleteTodoItemCommandHandler(_todoItemRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldDeleteTodoItem()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        var existingTodoItem = new TodoItem("Test Todo", "Test Description")
        {
            Id = todoId
        };
        existingTodoItem.ChangeStatus(TodoItemStatus.InProgress);
        var command = new DeleteTodoItemCommand()
        {
            Id = todoId
        };

        _todoItemRepositoryMock
            .Setup(x => x.GetAsync(todoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTodoItem);

        _todoItemRepositoryMock
            .Setup(x => x.DeleteAsync(existingTodoItem, true, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(existingTodoItem);
        result.Id.Should().Be(todoId);
        result.Status.Should().Be(TodoItemStatus.InProgress);

        _todoItemRepositoryMock.Verify(
            x => x.GetAsync(todoId, It.IsAny<CancellationToken>()),
            Times.Once);

        _todoItemRepositoryMock.Verify(
            x => x.DeleteAsync(existingTodoItem, true, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_CompletedTodoItem_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        var existingTodoItem = new TodoItem("Test Todo", "Test Description")
        {
            Id = todoId
        };
        existingTodoItem.ChangeStatus(TodoItemStatus.Completed);
        var command = new DeleteTodoItemCommand()
        {
            Id = todoId
        };

        _todoItemRepositoryMock
            .Setup(x => x.GetAsync(todoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTodoItem);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cannot delete a TodoItem that is already completed.");

        _todoItemRepositoryMock.Verify(
            x => x.GetAsync(todoId, It.IsAny<CancellationToken>()),
            Times.Once);

        _todoItemRepositoryMock.Verify(
            x => x.DeleteAsync(It.IsAny<TodoItem>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_NonExistentTodoItem_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        var command = new DeleteTodoItemCommand()
        {
            Id = todoId
        };

        _todoItemRepositoryMock
            .Setup(x => x.GetAsync(todoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TodoItem)null!);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"TodoItem with ID {todoId} not found.");

        _todoItemRepositoryMock.Verify(
            x => x.GetAsync(todoId, It.IsAny<CancellationToken>()),
            Times.Once);

        _todoItemRepositoryMock.Verify(
            x => x.DeleteAsync(It.IsAny<TodoItem>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()),
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
            x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _todoItemRepositoryMock.Verify(
            x => x.DeleteAsync(It.IsAny<TodoItem>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
} 