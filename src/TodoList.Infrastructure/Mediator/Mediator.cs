using Microsoft.Extensions.DependencyInjection;
using TodoList.Application;
using TodoList.Application.Commands;
using TodoList.Application.Queries;


namespace TodoList.Infrastructure.Mediator;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IUnitOfWork _unitOfWork;

    public Mediator(IServiceProvider serviceProvider, IUnitOfWork unitOfWork)
    {
        _serviceProvider = serviceProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
        var handler = _serviceProvider.GetRequiredService(handlerType);

        await _unitOfWork.BeginAsync();

        var result = await (Task<TResponse>)handlerType
            .GetMethod("Handle")!
            .Invoke(handler, [command, cancellationToken])!;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return result;
    }

    public async Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
        var handler = _serviceProvider.GetRequiredService(handlerType);

        await _unitOfWork.BeginAsync();

        var result = await (Task<TResponse>)handlerType
            .GetMethod("Handle")!
            .Invoke(handler, [query, cancellationToken])!;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return result;
    }
} 