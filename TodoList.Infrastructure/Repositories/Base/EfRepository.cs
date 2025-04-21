using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Common;
using TodoList.Domain.Repositories.Base;

namespace TodoList.Infrastructure.Repositories.Base;

public abstract class EfRepository<TEntity, TDbContext>(TDbContext dbContext)
    : EfReadOnlyRepository<TEntity, TDbContext>(dbContext),
        IRepository<TEntity>
    where TEntity : class, IEntity
    where TDbContext : DbContext
{
    public virtual async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await GetDbSet().AddAsync(entity, cancellationToken);

        if (autoSave)
        {
            await GetDbContext().SaveChangesAsync(cancellationToken);
        }

        return entity;
    }

    public virtual async Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        await GetDbSet().AddRangeAsync(entities, cancellationToken);

        if (autoSave)
        {
            await GetDbContext().SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        GetDbSet().Update(entity);

        if (autoSave)
        {
            await GetDbContext().SaveChangesAsync(cancellationToken);
        }

        return entity;
    }

    public virtual async Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        GetDbSet().UpdateRange(entities);

        if (autoSave)
        {
            await GetDbContext().SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (entity is ISoftDelete softDeleteEntity)
        {
            softDeleteEntity.IsDeleted = true;
        }
        else
        {
            GetDbSet().Remove(entity);
        }

        if (autoSave)
        {
            await GetDbContext().SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        if (entities.Any())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                foreach (var entity in entities.Cast<ISoftDelete>())
                {
                    entity.IsDeleted = true;
                }
            }
            else
            {
                GetDbSet().RemoveRange(entities);
            }

            if (autoSave)
            {
                await GetDbContext().SaveChangesAsync(cancellationToken);
            }
        }
    }

}

public abstract class EfRepository<TEntity, TKey, TDbContext>(TDbContext dbContext)
    : EfReadOnlyRepository<TEntity, TKey, TDbContext>(dbContext)
    where TEntity : class, IEntity<TKey>
    where TDbContext : DbContext
{
    public virtual async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await GetDbSet().AddAsync(entity, cancellationToken);

        if (autoSave)
        {
            await GetDbContext().SaveChangesAsync(cancellationToken);
        }

        return entity;
    }

    public virtual async Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        await GetDbSet().AddRangeAsync(entities, cancellationToken);

        if (autoSave)
        {
            await GetDbContext().SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        GetDbSet().Update(entity);

        if (autoSave)
        {
            await GetDbContext().SaveChangesAsync(cancellationToken);
        }

        return entity;
    }

    public virtual async Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        GetDbSet().UpdateRange(entities);

        if (autoSave)
        {
            await GetDbContext().SaveChangesAsync(cancellationToken);
        }
    }

    public virtual async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (entity is ISoftDelete softDeleteEntity)
        {
            softDeleteEntity.IsDeleted = true;
        }
        else
        {
            GetDbSet().Remove(entity);
        }

        if (autoSave)
        {
            await GetDbContext().SaveChangesAsync(cancellationToken);
        }
    }
    public virtual async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return;
        }

        await DeleteAsync(entity, autoSave, cancellationToken);
    }

    public virtual async Task DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        if (entities.Any())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                foreach (var entity in entities.Cast<ISoftDelete>())
                {
                    entity.IsDeleted = true;
                }
            }
            else
            {
                GetDbSet().RemoveRange(entities);
            }

            if (autoSave)
            {
                await GetDbContext().SaveChangesAsync(cancellationToken);
            }
        }
    }


    public virtual async Task DeleteManyAsync(IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entities = await GetDbSet().Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

        await DeleteManyAsync(entities, autoSave, cancellationToken);
    }
}