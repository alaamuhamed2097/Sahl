using DAL.ApplicationContext;
using DAL.Contracts.Repositories;
using DAL.Contracts.UnitOfWork;
using DAL.Repositories;
using Domains.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();
        private bool _disposed;
        private IDbContextTransaction _transaction;
        private readonly ILogger _logger;
        public int transactionsCount = 0;

        public UnitOfWork(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.TryGetValue(type, out var repo))
            {
                repo = new Repository<T>(_context, _logger);
                _repositories[type] = repo;
            }
            return (IRepository<T>)repo;
        }

        public ITableRepository<TD> TableRepository<TD>() where TD : BaseEntity
        {
            var type = typeof(TD);
            if (!_repositories.TryGetValue(type, out var repo))
            {
                repo = new TableRepository<TD>(_context, _logger);
                _repositories[type] = repo;
            }
            return (ITableRepository<TD>)repo;
        }

        public DbContext GetContext() => _context; // Add this method

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (transactionsCount == 0)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
            }

            ++transactionsCount;
            return _transaction;
        }

        public async Task<int> CommitAsync()
        {
            int result = 0;

            if (transactionsCount <= 1)
            {
                result = await _context.SaveChangesAsync();
                _transaction?.Commit();
            }

            --transactionsCount;
            return result;
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
            _transaction = null!;
        }

        public async Task DisposeAsync()
        {
            if (!_disposed)
            {
                _transaction?.Dispose();
                await _context.DisposeAsync();
                _repositories.Clear();
                _disposed = true;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _transaction?.Dispose();
                _context.Dispose();
                _repositories.Clear();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
