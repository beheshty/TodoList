using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Transactions;
using TodoList.Infrastructure.Data;

namespace TodoList.Infrastructure.UnitOfWork
{
    //todo "IDisposable" should be implemented correctly
    public class UnitOfWork(TodoListDbContext dbContext) : IUnitOfWork
    {
        private readonly Guid _id = Guid.NewGuid();
        public Guid Id => _id;
        private TransactionScope _transactionScope;
        private IServiceScope? _syncScope;
        private AsyncServiceScope? _asyncScope;

        public bool IsTransactional { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsBegan { get; private set; }

        public async Task BeginAsync(bool isTransactional = true)
        {
            if (IsBegan)
                return;
            if (isTransactional)
            {
                var connection = dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }
            }
            IsTransactional = isTransactional;
            IsBegan = true;
        }

        public void Begin(bool isTransactional = true)
        {
            if (IsBegan)
                return;
            if (isTransactional)
            {
                _transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                var connection = dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

            }
            IsTransactional = isTransactional;
            IsBegan = true;
        }


        public async Task CompleteAsync(CancellationToken cancellationToken = default)
        {
            CheckIfTheUowIsBegan();
            if (IsCompleted)
                return;

            await SaveChangesAsync(cancellationToken);
            _transactionScope?.Complete();

            IsCompleted = true;
        }

        /// <summary>
        /// WARNING: Using this method will prevent domain events from being raised.
        /// If you need domain events to be raised, use the asynchronous method <see cref="CompleteAsync"/>.
        /// </summary>
        public void Complete()
        {
            CheckIfTheUowIsBegan();
            if (IsCompleted)
                return;

            SaveChanges();
            _transactionScope?.Complete();
            IsCompleted = true;
        }

        private void CheckIfTheUowIsBegan()
        {
            if (!IsBegan)
            {
                throw new InvalidOperationException("The UOW should begin before completing it!");
            }
        }

        public void SetScope(IServiceScope scope)
        {
            _syncScope = scope;
        }

        public void SetAsyncScope(AsyncServiceScope asyncScope)
        {
            _asyncScope = asyncScope;
        }

        public void Dispose()
        {
            if (!IsCompleted)
            {
                Rollback();
            }

            InternalDispose();
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            if (!IsCompleted)
            {
                Rollback();
            }

            await InternalDisposeAsync();
            GC.SuppressFinalize(this);
        }

        private void InternalDispose()
        {
            _transactionScope?.Dispose();
            _syncScope?.Dispose();
        }

        private async ValueTask InternalDisposeAsync()
        {
            _transactionScope?.Dispose();

            if (_asyncScope.HasValue)
            {
                await _asyncScope.Value.DisposeAsync();
            }
        }

        public void Rollback()
        {
            if (!IsCompleted)
            {
                if (IsTransactional)
                {
                    _transactionScope?.Dispose();
                }
                IsCompleted = true;
            }
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }
    }
}
