using FluentAssertions;
using Moq;
using TodoList.Application.Queries.TodoItems;
using TodoList.Application.Tests.Common;
using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Repositories.TodoItems;

namespace TodoList.Application.Tests.Queries.TodoItems;

public class GetTodoItemByIdQueryHandlerTests : TestBase
{
    private readonly GetTodoItemByIdQueryHandler _handler;
    private readonly Mock<ITodoItemRepository> _todoItemRepositoryMock;

    public GetTodoItemByIdQueryHandlerTests()
    {
        _todoItemRepositoryMock = new Mock<ITodoItemRepository>();
        _handler = new GetTodoItemByIdQueryHandler(_todoItemRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnTodoItem()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        var query = new GetTodoItemByIdQuery()
        {
            Id = todoId
        };
        var expectedTodoItem = new TodoItem("Test Todo", "Test Description");

        _todoItemRepositoryMock
            .Setup(x => x.GetAsync(todoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedTodoItem);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expectedTodoItem);
        result.Title.Should().Be(expectedTodoItem.Title);
        result.Description.Should().Be(expectedTodoItem.Description);

        _todoItemRepositoryMock.Verify(
            x => x.GetAsync(todoId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_NullQuery_ShouldThrowArgumentNullException()
    {
        // Act
        var act = () => _handler.Handle(null!, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
        _todoItemRepositoryMock.Verify(
            x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_InvalidId_ShouldReturnNull()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        var query = new GetTodoItemByIdQuery()
        {
            Id = todoId
        };

        _todoItemRepositoryMock
            .Setup(x => x.GetAsync(todoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TodoItem)null!);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();

        _todoItemRepositoryMock.Verify(
            x => x.GetAsync(todoId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
} 