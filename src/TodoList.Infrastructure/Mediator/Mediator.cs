using Microsoft.Extensions.DependencyInjection;
using TodoList.Application;
using TodoList.Application.Commands;
using TodoList.Application.Queries;


namespace TodoList.Infrastructure.Mediator;

public class Mediator(IServiceProvider serviceProvider, IUnitOfWork unitOfWork) : IMediator
{
    public async Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
        var handler = serviceProvider.GetRequiredService(handlerType);

        await unitOfWork.BeginAsync();

        var result = await (Task<TResponse>)handlerType
            .GetMethod("Handle")!
            .Invoke(handler, [command, cancellationToken])!;

        await unitOfWork.CompleteAsync(cancellationToken);

        return result;
    }

    public async Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
        var handler = serviceProvider.GetRequiredService(handlerType);

        await unitOfWork.BeginAsync();

        var result = await (Task<TResponse>)handlerType
            .GetMethod("Handle")!
            .Invoke(handler, [query, cancellationToken])!;

        await unitOfWork.CompleteAsync(cancellationToken);

        return result;
    }
} 