using DAL.Models;
using System.Linq.Expressions;

namespace DAL.Contracts.Repositories
{
    /// <summary>
    /// Base repository interface for all entity operations
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IRepository<T> where T : class
    {
        // ============================================
        // READ OPERATIONS - Basic
        // ============================================

        /// <summary>
        /// Retrieves all entities asynchronously
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves entities based on a predicate
        /// </summary>
        Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>> predicate = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves entities with advanced filtering, ordering, and eager loading
        /// </summary>
        Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? take = null,
            string includeProperties = "",
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] thenIncludeProperties);

        /// <summary>
        /// Retrieves paginated data
        /// </summary>
        Task<PaginatedDataModel<T>> GetPageAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds the first entity matching the predicate
        /// </summary>
        Task<T> FindAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets IQueryable for advanced LINQ queries - للأداء الأفضل
        /// Use this when you need to compose complex queries
        /// </summary>
        IQueryable<T> GetQueryable();

        // ============================================
        // READ OPERATIONS - Count & Existence
        // ============================================

        /// <summary>
        /// Counts entities matching the predicate
        /// </summary>
        Task<int> CountAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if an entity exists based on a key-value pair
        /// </summary>
        Task<bool> IsExistsAsync<TValue>(
            string key,
            TValue value,
            CancellationToken cancellationToken = default);

        // ============================================
        // STORED PROCEDURES & RAW SQL
        // ============================================

        /// <summary>
        /// Executes a stored procedure and maps results to entity
        /// </summary>
        Task<List<T>> ExecuteStoredProcedureAsync(
            string storedProcedureName,
            CancellationToken cancellationToken = default,
            params Microsoft.Data.SqlClient.SqlParameter[] parameters);

        /// <summary>
        /// Executes a SQL function and returns a scalar value
        /// </summary>
        Task<TResult> ExecuteScalarSqlFunctionAsync<TResult>(
            string sqlFunctionQuery,
            CancellationToken cancellationToken = default,
            params object[] parameters);

        /// <summary>
        /// Executes raw SQL and returns a scalar value
        /// </summary>
        Task<TResult> ExecuteScalarRawSqlAsync<TResult>(
            string sqlQuery,
            CancellationToken cancellationToken = default,
            params object[] parameters);
    }
}
