using Microsoft.Extensions.DependencyInjection;
using TodoList.Application;
using TodoList.Application.Commands;
using TodoList.Application.Commands.TodoItems;
using TodoList.Application.Queries;
using TodoList.Application.Queries.TodoItems;


namespace TodoList.Infrastructure.Mediator;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();

        services.AddCommand<CreateTodoItemCommandHandler>();
        services.AddCommand<UpdateTodoItemCommandHandler>();
        services.AddCommand<UpdateTodoItemStatusCommandHandler>();
        services.AddCommand<DeleteTodoItemCommandHandler>();

        services.AddQuery<GetTodoItemsQueryHandler>();
        services.AddQuery<GetTodoItemByIdQueryHandler>();

        return services;
    }

    private static IServiceCollection AddCommand<TCommandHandler>(this IServiceCollection services)
        where TCommandHandler : class
    {
        var handlerInterface = typeof(TCommandHandler).GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));

        if (handlerInterface == null)
        {
            throw new ArgumentException($"{typeof(TCommandHandler).Name} does not implement ICommandHandler<,>", nameof(TCommandHandler));
        }

        services.AddScoped(handlerInterface, typeof(TCommandHandler));
        return services;
    }

    private static IServiceCollection AddQuery<TQueryHandler>(this IServiceCollection services)
        where TQueryHandler : class
    {
        var handlerInterface = typeof(TQueryHandler).GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));

        if (handlerInterface == null)
        {
            throw new ArgumentException($"{typeof(TQueryHandler).Name} does not implement IQueryHandler<,>", nameof(TQueryHandler));
        }

        services.AddScoped(handlerInterface, typeof(TQueryHandler));
        return services;
    }
} 