using DAL.Contracts.Repositories;
using Domains.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL.Contracts.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class;
        ITableRepository<TD> TableRepository<TD>() where TD : BaseEntity;
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> CommitAsync();
        Task RollbackAsync();
        Task DisposeAsync(); // Changed from Task ValueTask to Task for simplicity
        DbContext GetContext(); // Add this method
    }
}
