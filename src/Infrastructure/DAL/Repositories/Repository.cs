using DAL.ApplicationContext;
using DAL.Contracts.Repositories;
using DAL.Exceptions;
using DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq.Expressions;
using System.Text;

namespace DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;

        // Constants for SQL error codes
        private const int SQL_CUSTOM_ERROR_NUMBER = 50000;

        protected DbSet<T> DbSet => _dbContext.Set<T>();

        public Repository(ApplicationDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "Database context cannot be null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger instance cannot be null.");
        }

        /// <summary>
        /// Retrieves all entities asynchronously.
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await DbSet.AsNoTracking().ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(GetAllAsync), $"Error occurred while retrieving all entities of type {typeof(T).Name}.", ex);
                return Enumerable.Empty<T>(); // Never reached due to throw in HandleException
            }
        }

        /// <summary>
        /// Retrieves entities based on a predicate asynchronously.
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>> predicate = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var query = predicate == null
                    ? DbSet.AsNoTracking()
                    : DbSet.Where(predicate).AsNoTracking();

                return await query.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(GetAsync), $"Error occurred while filtering entities of type {typeof(T).Name}.", ex);
                return Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// Retrieves entities with optional filtering, ordering, and eager loading asynchronously.
        /// </summary>
        public virtual async Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? take = null,
            string includeProperties = "",
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] thenIncludeProperties)
        {
            try
            {
                IQueryable<T> query = DbSet.AsNoTracking();

                if (predicate != null)
                {
                    query = query.Where(predicate);
                }

                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty.Trim());
                    }

                    if (thenIncludeProperties?.Length > 0)
                    {
                        foreach (var thenIncludeProperty in thenIncludeProperties)
                        {
                            query = query.Include(thenIncludeProperty);
                        }
                    }
                }

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                if (take.HasValue && take.Value > 0)
                {
                    query = query.Take(take.Value);
                }

                return await query.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(GetAsync), $"Error occurred while retrieving entities of type {typeof(T).Name} with advanced options.", ex);
                return Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// Retrieves paginated data asynchronously.
        /// </summary>
        public virtual async Task<PaginatedDataModel<T>> GetPageAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                ValidatePaginationParameters(pageNumber, pageSize);

                IQueryable<T> query = DbSet.AsNoTracking();

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                int totalCount = await query.CountAsync(cancellationToken);

                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                var data = await query.ToListAsync(cancellationToken);

                return new PaginatedDataModel<T>(data, totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred in {nameof(GetPageAsync)} method for entity type {typeof(T).Name}.");
                throw new DataAccessException(
                    $"Error occurred in {nameof(GetPageAsync)} method for entity type {typeof(T).Name}.",
                    ex,
                    _logger
                );
            }
        }

        /// <summary>
        /// Finds the first entity matching the predicate asynchronously.
        /// </summary>
        public virtual async Task<T> FindAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await DbSet.FirstOrDefaultAsync(predicate, cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(FindAsync), $"Error occurred while finding an entity of type {typeof(T).Name}.", ex);
                return null;
            }
        }

        /// <summary>
        /// Gets IQueryable for advanced LINQ queries - للأداء الأفضل
        /// Use this when you need to compose complex queries
        /// </summary>
        public virtual IQueryable<T> GetQueryable()
        {
            return DbSet.AsNoTracking();
        }

        /// <summary>
        /// Counts entities matching the predicate asynchronously.
        /// </summary>
        public virtual async Task<int> CountAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await DbSet.CountAsync(predicate, cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(CountAsync), $"Error occurred while counting entities of type {typeof(T).Name}.", ex);
                return 0;
            }
        }

        /// <summary>
        /// Checks if an entity exists based on a key-value pair asynchronously.
        /// </summary>
        public async Task<bool> IsExistsAsync<TValue>(
            string key,
            TValue value,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                    throw new ArgumentException("Key cannot be null or empty.", nameof(key));

                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, key);
                var constant = Expression.Constant(value);
                var equality = Expression.Equal(property, constant);
                var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

                return await DbSet.AnyAsync(lambda, cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(IsExistsAsync), $"Error occurred while checking existence for entity type {typeof(T).Name}.", ex);
                return false;
            }
        }

        /// <summary>
        /// Executes a stored procedure and maps the results to the entity class asynchronously.
        /// </summary>
        public async Task<List<T>> ExecuteStoredProcedureAsync(
            string storedProcedureName,
            CancellationToken cancellationToken = default,
            params SqlParameter[] parameters)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(storedProcedureName))
                    throw new ArgumentException("Stored procedure name cannot be null or empty.", nameof(storedProcedureName));

                var safeParameters = parameters?.Select(param =>
                {
                    if (param.Value == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    return param;
                }).ToArray() ?? Array.Empty<SqlParameter>();

                var commandText = BuildStoredProcedureCommand(storedProcedureName, safeParameters);

                return await DbSet.FromSqlRaw(commandText, safeParameters)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (SqlException sqlEx) when (sqlEx.Number == SQL_CUSTOM_ERROR_NUMBER)
            {
                _logger.Error(sqlEx, $"{nameof(ExecuteStoredProcedureAsync)}: Custom error from stored procedure: {storedProcedureName}");
                throw new DataAccessException(sqlEx.Message, sqlEx, _logger);
            }
            catch (Exception ex)
            {
                HandleException(nameof(ExecuteStoredProcedureAsync), $"Error occurred while executing stored procedure.", ex);
                return new List<T>();
            }
        }

        /// <summary>
        /// Builds a safe stored procedure command with parameters.
        /// </summary>
        private string BuildStoredProcedureCommand(string storedProcedureName, SqlParameter[] parameters)
        {
            var commandBuilder = new StringBuilder();
            commandBuilder.Append($"EXEC {storedProcedureName}");

            if (parameters?.Length > 0)
            {
                commandBuilder.Append(" ");
                var paramMappings = parameters.Select(p => $"{p.ParameterName}={p.ParameterName}");
                commandBuilder.Append(string.Join(", ", paramMappings));
            }

            return commandBuilder.ToString();
        }

        /// <summary>
        /// Executes a custom SQL function and returns a single scalar value asynchronously.
        /// </summary>
        public async Task<TResult> ExecuteScalarSqlFunctionAsync<TResult>(
            string sqlFunctionQuery,
            CancellationToken cancellationToken = default,
            params object[] parameters)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sqlFunctionQuery))
                    throw new ArgumentException("SQL function query cannot be null or empty.", nameof(sqlFunctionQuery));

                return await ExecuteScalarAsync<TResult>(sqlFunctionQuery, cancellationToken, parameters);
            }
            catch (Exception ex)
            {
                HandleException(nameof(ExecuteScalarSqlFunctionAsync), $"Error occurred while executing scalar SQL function.", ex);
                return default;
            }
        }

        /// <summary>
        /// Executes a raw SQL query and returns a single scalar value asynchronously.
        /// </summary>
        public async Task<TResult> ExecuteScalarRawSqlAsync<TResult>(
            string sqlQuery,
            CancellationToken cancellationToken = default,
            params object[] parameters)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sqlQuery))
                    throw new ArgumentException("SQL query cannot be null or empty.", nameof(sqlQuery));

                return await ExecuteScalarAsync<TResult>(sqlQuery, cancellationToken, parameters);
            }
            catch (Exception ex)
            {
                HandleException(nameof(ExecuteScalarRawSqlAsync), $"Error occurred while executing scalar raw SQL query.", ex);
                return default;
            }
        }

        /// <summary>
        /// Internal method to execute a scalar query using ADO.NET asynchronously.
        /// </summary>
        private async Task<TResult> ExecuteScalarAsync<TResult>(
            string sql,
            CancellationToken cancellationToken = default,
            params object[] parameters)
        {
            var connection = _dbContext.Database.GetDbConnection();

            if (connection == null)
                throw new InvalidOperationException("Database connection is not available.");

            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = System.Data.CommandType.Text;

                if (parameters?.Length > 0)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var parameter = command.CreateParameter();
                        parameter.ParameterName = $"@p{i}";
                        parameter.Value = parameters[i] ?? DBNull.Value;
                        command.Parameters.Add(parameter);
                    }
                }

                if (command.Connection.State == System.Data.ConnectionState.Closed)
                {
                    await command.Connection.OpenAsync(cancellationToken);
                }

                try
                {
                    var result = await command.ExecuteScalarAsync(cancellationToken);
                    return result == DBNull.Value ? default : (TResult)Convert.ChangeType(result, typeof(TResult));
                }
                finally
                {
                    if (command.Connection.State == System.Data.ConnectionState.Open)
                    {
                        await command.Connection.CloseAsync();
                    }
                }
            }
        }

        /// <summary>
        /// Logs and throws exceptions with detailed information.
        /// </summary>
        protected void HandleException(string methodName, string message, Exception ex)
        {
            _logger.Error(ex, $"[{methodName}] {message}");
            throw new DataAccessException(message, ex, _logger);
        }

        /// <summary>
        /// Validates pagination parameters.
        /// </summary>
        protected void ValidatePaginationParameters(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("Page number must be greater than zero.", nameof(pageNumber));

            if (pageSize <= 0)
                throw new ArgumentException("Page size must be greater than zero.", nameof(pageSize));
        }
    }
}