using FluentAssertions;
using Moq;
using TodoList.Application.Commands.TodoItems;
using TodoList.Application.Tests.Common;
using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Repositories.TodoItems;

namespace TodoList.Application.Tests.Commands.TodoItems;

public class CreateTodoItemCommandHandlerTests : TestBase
{
    private readonly CreateTodoItemCommandHandler _handler;
    private readonly Mock<ITodoItemRepository> _todoItemRepositoryMock;

    public CreateTodoItemCommandHandlerTests()
    {
        _todoItemRepositoryMock = new Mock<ITodoItemRepository>();
        _handler = new CreateTodoItemCommandHandler(_todoItemRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateTodoItem()
    {
        // Arrange
        var command = new CreateTodoItemCommand
        {
            Title = "Test Todo",
            Description = "Test Description"
        };

        _todoItemRepositoryMock
            .Setup(x => x.InsertAsync(It.IsAny<TodoItem>(), true, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TodoItem item, bool _, CancellationToken _) => item);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(command.Title);
        result.Description.Should().Be(command.Description);
        result.Status.Should().Be(TodoItemStatus.InProgress);
        result.CreationTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

        _todoItemRepositoryMock.Verify(
            x => x.InsertAsync(It.IsAny<TodoItem>(), true, It.IsAny<CancellationToken>()),
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
            x => x.InsertAsync(It.IsAny<TodoItem>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
} 