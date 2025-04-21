using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TodoList.Domain.Common;
using TodoList.Domain.Exceptions;
using TodoList.Domain.Repositories.Base;

namespace TodoList.Infrastructure.Repositories.Base;

public abstract class EfReadOnlyRepository<TEntity, TDbContext>(TDbContext dbContext) : IReadOnlyRepository<TEntity>
    where TEntity : class, IEntity
    where TDbContext : DbContext
{

    public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await GetDbSet().AnyAsync(cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await GetDbSet().AnyAsync(predicate, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await GetDbSet().LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
    {
        return await GetDbSet().ToListAsync(cancellationToken);
    }

    public TDbContext GetDbContext()
    {
        return dbContext;
    }

    public DbSet<TEntity> GetDbSet()
    {
        return GetDbContext().Set<TEntity>();
    }
}

public abstract class EfReadOnlyRepository<TEntity, TKey, TDbContext>(TDbContext dbContext)
    : IReadOnlyRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TDbContext : DbContext
{

    public virtual async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, cancellationToken);

        return entity ?? throw new EntityNotFoundException(typeof(TEntity), id);
    }

    public virtual async Task<TEntity?> FindAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await GetDbSet().FindAsync(id, cancellationToken);
    }

    public TDbContext GetDbContext()
    {
        return dbContext;
    }

    public DbSet<TEntity> GetDbSet()
    {
        return GetDbContext().Set<TEntity>();
    }

    public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await GetDbSet().AnyAsync(cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await GetDbSet().AnyAsync(predicate, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await GetDbSet().LongCountAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
    {
        return await GetDbSet().ToListAsync(cancellationToken);
    }
}