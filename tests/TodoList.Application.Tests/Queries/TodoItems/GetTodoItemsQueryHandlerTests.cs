using FluentAssertions;
using Moq;
using TodoList.Application.Queries.TodoItems;
using TodoList.Application.Tests.Common;
using TodoList.Domain.Entities.TodoItems;
using TodoList.Domain.Filters;
using TodoList.Domain.Repositories.TodoItems;

namespace TodoList.Application.Tests.Queries.TodoItems;

public class GetTodoItemsQueryHandlerTests : TestBase
{
    private readonly GetTodoItemsQueryHandler _handler;
    private readonly Mock<ITodoItemRepository> _todoItemRepositoryMock;

    public GetTodoItemsQueryHandlerTests()
    {
        _todoItemRepositoryMock = new Mock<ITodoItemRepository>();
        _handler = new GetTodoItemsQueryHandler(_todoItemRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnTodoItems()
    {
        // Arrange
        var query = new GetTodoItemsQuery();
        var expectedTodoItems = new List<TodoItem>
        {
            new TodoItem("Test Todo 1", "Test Description 1"),
            new TodoItem("Test Todo 2", "Test Description 2")
        };

        _todoItemRepositoryMock
            .Setup(x => x.GetListAsync(It.IsAny<TodoItemFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedTodoItems);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedTodoItems);
        result.Should().HaveCount(2);

        _todoItemRepositoryMock.Verify(
            x => x.GetListAsync(It.IsAny<TodoItemFilter>(), It.IsAny<CancellationToken>()),
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
            x => x.GetListAsync(It.IsAny<TodoItemFilter>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
} 