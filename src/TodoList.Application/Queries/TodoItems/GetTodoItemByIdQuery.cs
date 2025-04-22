using TodoList.Domain.Entities.TodoItems;

namespace TodoList.Application.Queries.TodoItems;

public class GetTodoItemByIdQuery : IQuery<TodoItem>
{
    public GetTodoItemByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}