using DAL.Models;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;

namespace DAL.Contracts.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate = null);
        IEnumerable<T> Get(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? take = null,
            string includeProperties = "",
            params Expression<Func<T, object>>[] thenIncludeProperties);
        public PaginatedDataModel<T> GetPage(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        T Find(Expression<Func<T, bool>> predicate);
        int Count(Expression<Func<T, bool>> predicate);
        bool IsExists<TValue>(string key, TValue value);
        public List<T> ExecuteStoredProcedure(string storedProcedureName, params SqlParameter[] parameters);
        TResult ExecuteScalarSqlFunction<TResult>(string sqlFunctionQuery, params object[] parameters);
        TResult ExecuteScalarRawSql<TResult>(string sqlQuery, params object[] parameters);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate = null);
        Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? take = null,
            string includeProperties = "",
            params Expression<Func<T, object>>[] thenIncludeProperties);
        Task<PaginatedDataModel<T>> GetPageAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task<bool> IsExistsAsync<TValue>(string key, TValue value);
        Task<List<T>> ExecuteStoredProcedureAsync(string storedProcedureName, params SqlParameter[] parameters);
        Task<TResult> ExecuteScalarSqlFunctionAsync<TResult>(string sqlFunctionQuery, params object[] parameters);
        Task<TResult> ExecuteScalarRawSqlAsync<TResult>(string sqlQuery, params object[] parameters);
    }
}
