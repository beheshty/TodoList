using Microsoft.Extensions.DependencyInjection;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    Guid Id { get; }
    bool IsCompleted { get; }
    bool IsBegan { get; }
    bool IsTransactional { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    void SaveChanges();
    Task CompleteAsync(CancellationToken cancellationToken = default);
    void Complete();
    Task BeginAsync(bool isTransactional = true);
    void Begin(bool isTransactional = true);
    void SetScope(IServiceScope scope);
    void SetAsyncScope(AsyncServiceScope asyncScope);
    void Rollback();
}