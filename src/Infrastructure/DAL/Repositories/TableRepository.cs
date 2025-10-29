using DAL.ApplicationContext;
using DAL.Contracts.Repositories;
using DAL.Exceptions;
using DAL.Models;
using DAL.ResultModels;
using Domains.Entities.Base;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public class TableRepository<T> : Repository<T>, ITableRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;

        protected DbSet<T> DbSet => _dbContext.Set<T>();

        public TableRepository(ApplicationDbContext dbContext, ILogger logger) : base(dbContext, logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "Database context cannot be null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger instance cannot be null.");
        }

        /// <summary>
        /// Retrieves all active entities (CurrentState == 1).
        /// </summary>
        public override IEnumerable<T> GetAll()
        {
            try
            {
                return DbSet.AsNoTracking().Where(e => e.CurrentState == 1).ToList();
            }
            catch (Exception ex)
            {
                HandleException(nameof(GetAll), $"Error occurred while retrieving all active entities of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves entities based on a predicate, filtering only active records.
        /// </summary>
        public override IEnumerable<T> Get(Expression<Func<T, bool>> predicate = null)
        {
            try
            {
                if (predicate == null)
                    return DbSet.AsNoTracking().Where(e => e.CurrentState == 1).ToList();

                return DbSet.Where(predicate)?.Where(e => e.CurrentState == 1).AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                HandleException(nameof(Get), $"Error occurred while filtering active entities of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves paginated data.
        /// </summary>
        public override PaginatedDataModel<T> GetPage(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            try
            {
                IQueryable<T> query = DbSet.AsNoTracking();

                // Apply the filter if it exists
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                // Apply ordering if provided
                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                // Get the total count before pagination
                int totalCount = query.Count();

                // Apply pagination
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                // Execute the query and return the paginated data
                var data = query.ToList();

                return new PaginatedDataModel<T>(data, totalCount);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(
                    $"Error occurred in {nameof(GetPage)} method for entity type {typeof(T).Name}.",
                    ex,
                    _logger
                );
            }
        }

        /// <summary>
        /// Finds an entity by its ID.
        /// </summary>
        public T FindById(Guid id)
        {
            try
            {
                var data = DbSet.Find(id);
                if (data == null)
                    throw new NotFoundException($"Entity of type {typeof(T).Name} with ID {id} not found.", _logger);

                return data;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                HandleException(nameof(FindById), $"Error occurred while finding an entity of type {typeof(T).Name} with ID {id}.", ex);
                throw;
            }
        }


        /// <summary>
        /// Saves or updates an entity based on its key.
        /// </summary>
        public bool Save(T model, Guid userId)
        {
            try
            {
                if (model.Id == Guid.Empty)
                    return Create(model, userId, out _);
                else
                    return Update(model, userId, out _);
            }
            catch (Exception ex)
            {
                HandleException(nameof(Save), $"Error occurred while saving or updating an entity of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Saves or updates an entity based on its key and outputs the ID of the saved entity.
        /// </summary>
        public bool Save(T model, Guid userId, out Guid id)
        {
            try
            {
                if (model.Id == Guid.Empty)
                    return Create(model, userId, out id);
                else
                    return Update(model, userId, out id);
            }
            catch (Exception ex)
            {
                HandleException(nameof(Save), $"Error occurred while saving or updating an entity of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        public bool Create(T model, Guid creatorId, out Guid id)
        {
            try
            {
                id = Guid.NewGuid();

                model.Id = id;
                model.CreatedDateUtc = DateTime.UtcNow;
                model.CreatedBy = creatorId;
                model.CurrentState = 1;

                DbSet.Add(model);
                return _dbContext.SaveChanges() > 0;
            }
            catch (DbUpdateException dbEx)
            {
                HandleException(nameof(Create), $"Conflict error while creating an entity of type {typeof(T).Name}.", dbEx);
                throw;
            }
            catch (Exception ex)
            {
                HandleException(nameof(Create), $"Error occurred while creating an entity of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        public bool Update(T model, Guid updaterId, out Guid id)
        {
            try
            {
                var existingEntity = DbSet.AsNoTracking().FirstOrDefault(e => e.Id == model.Id);
                id = model.Id;

                if (existingEntity == null)
                    throw new DataAccessException($"Entity with key {id} not found.", _logger);

                model.UpdatedDateUtc = DateTime.UtcNow;
                model.UpdatedBy = updaterId;
                model.CreatedBy = existingEntity.CreatedBy;
                model.CurrentState = existingEntity.CurrentState;
                model.CreatedDateUtc = existingEntity.CreatedDateUtc;

                DbSet.Entry(model).State = EntityState.Modified;

                return _dbContext.SaveChanges() > 0;
            }
            catch (DbUpdateConcurrencyException concurrencyEx)
            {
                HandleException(nameof(Update), $"Concurrency error while updating an entity of type {typeof(T).Name}, ID {model.Id}.", concurrencyEx);
                throw;
            }
            catch (Exception ex)
            {
                HandleException(nameof(Update), $"Error occurred while updating an entity of type {typeof(T).Name}, ID {model.Id}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Updates the CurrentState of an entity.
        /// </summary>
        public bool UpdateCurrentState(Guid entityId, Guid updaterId, int newValue = 0)
        {
            try
            {
                var entity = DbSet.Find(entityId);

                if (entity == null)
                    throw new NotFoundException($"Entity of type {typeof(T).Name} with ID {entityId} not found.", _logger);

                entity.CurrentState = newValue;
                entity.UpdatedDateUtc = DateTime.UtcNow;
                entity.UpdatedBy = updaterId;

                DbSet.Update(entity);
                return _dbContext.SaveChanges() > 0;
            }
            catch (DbUpdateException dbEx)
            {
                HandleException(nameof(UpdateCurrentState), $"Database update error while updating CurrentState for entity type {typeof(T).Name}, ID {entityId}.", dbEx);
                throw;
            }
            catch (Exception ex)
            {
                HandleException(nameof(UpdateCurrentState), $"Error occurred while updating CurrentState for entity type {typeof(T).Name}, ID {entityId}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Permanently deletes an entity by its ID.
        /// </summary>
        public bool HardDelete(Guid id)
        {
            try
            {
                var entityToDelete = DbSet.SingleOrDefault(e => e.Id == id);

                if (entityToDelete == null)
                    throw new DataAccessException($"Entity with key {id} not found.", _logger);

                DbSet.Remove(entityToDelete);

                return _dbContext.SaveChanges() > 0;
            }
            catch (DbUpdateConcurrencyException concurrencyEx)
            {
                HandleException(nameof(HardDelete), $"Concurrency error while hard deleting an entity of type {typeof(T).Name}, ID {id}.", concurrencyEx);
                throw;
            }
            catch (Exception ex)
            {
                HandleException(nameof(HardDelete), $"Error occurred while hard deleting an entity of type {typeof(T).Name}, ID {id}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Saves all pending changes to the database.
        /// </summary>
        public bool SaveChange()
        {
            try
            {
                return _dbContext.SaveChanges() > 0;
            }
            catch (DbUpdateConcurrencyException concurrencyEx)
            {
                HandleException(nameof(SaveChange), $"Concurrency error while saving changes for entity type {typeof(T).Name}.", concurrencyEx);
                throw;
            }
            catch (Exception ex)
            {
                HandleException(nameof(SaveChange), $"Error occurred while saving changes for entity type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Logs and rethrows exceptions with detailed information.
        /// </summary>
        public bool AddRange(IEnumerable<T> entities, Guid userId)
        {
            try
            {

                var utcNow = DateTime.UtcNow;

                foreach (var entity in entities)
                {
                    entity.Id = Guid.NewGuid();
                    entity.CreatedDateUtc = utcNow;
                    entity.CreatedBy = userId;
                    entity.CurrentState = 1;
                }

                DbSet.AddRange(entities);
                var changes = _dbContext.SaveChanges() > 0;

                return changes;
            }
            catch (Exception ex)
            {
                HandleException(nameof(AddRange), $"Error occurred while adding multiple entities of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all active entities (CurrentState == 1).
        /// </summary>
        public override async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await DbSet.AsNoTracking().Where(e => e.CurrentState == 1).ToListAsync();
            }
            catch (Exception ex)
            {
                HandleException(nameof(GetAllAsync), $"Error occurred while retrieving all active entities of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves entities based on a predicate, filtering only active records.
        /// </summary>
        public override async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate = null)
        {
            try
            {
                if (predicate == null)
                    return await DbSet.AsNoTracking().Where(e => e.CurrentState == 1).ToListAsync();

                return await DbSet.Where(predicate)?.Where(e => e.CurrentState == 1).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                HandleException(nameof(GetAsync), $"Error occurred while filtering active entities of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves paginated data.
        /// </summary>
        public override async Task<PaginatedDataModel<T>> GetPageAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            try
            {
                IQueryable<T> query = DbSet.AsNoTracking();

                // Apply the filter if it exists
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                // Apply ordering if provided
                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                // Get the total count before pagination
                int totalCount = await query.CountAsync();

                // Apply pagination
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                // Execute the query and return the paginated data
                var data = await query.ToListAsync();

                return new PaginatedDataModel<T>(data, totalCount);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(
                    $"Error occurred in {nameof(GetPageAsync)} method for entity type {typeof(T).Name}.",
                    ex,
                    _logger
                );
            }
        }

        /// <summary>
        /// Finds an entity by its ID.
        /// </summary>
        public async Task<T> FindByIdAsync(Guid id)
        {
            try
            {
                var data = await DbSet.FindAsync(id);
                if (data == null)
                    throw new NotFoundException($"Entity of type {typeof(T).Name} with ID {id} not found.", _logger);

                return data;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                HandleException(nameof(FindByIdAsync), $"Error occurred while finding an entity of type {typeof(T).Name} with ID {id}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Saves or updates an entity based on its key and outputs the ID of the saved entity.
        /// </summary>
        public async Task<SaveResult> SaveAsync(T model, Guid userId)
        {
            try
            {
                if (model.Id == Guid.Empty)
                    return await CreateAsync(model, userId);
                else
                    return await UpdateAsync(model, userId);
            }
            catch (Exception ex)
            {
                HandleException(nameof(SaveAsync), $"Error occurred while saving or updating an entity of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        public async Task<SaveResult> CreateAsync(T model, Guid creatorId)
        {
            try
            {
                var id = Guid.NewGuid();

                model.Id = id;
                model.CreatedDateUtc = DateTime.UtcNow;
                model.CreatedBy = creatorId;
                model.CurrentState = 1;

                await DbSet.AddAsync(model);
                var result = await _dbContext.SaveChangesAsync() > 0;
                if (!result)
                    throw new DataAccessException($"Failed to create entity of type {typeof(T).Name}.", _logger);
                return new SaveResult() { Success = true, Id = id };
            }
            catch (DbUpdateException dbEx)
            {
                HandleException(nameof(CreateAsync), $"Conflict error while creating an entity of type {typeof(T).Name}.", dbEx);
                throw;
            }
            catch (Exception ex)
            {
                HandleException(nameof(CreateAsync), $"Error occurred while creating an entity of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        public async Task<SaveResult> UpdateAsync(T model, Guid updaterId)
        {
            try
            {
                var existingEntity = await DbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == model.Id);

                if (existingEntity == null)
                    throw new DataAccessException($"Entity with key {model.Id} not found.", _logger);

                model.UpdatedDateUtc = DateTime.UtcNow;
                model.UpdatedBy = updaterId;
                model.CreatedBy = existingEntity.CreatedBy;
                model.CurrentState = existingEntity.CurrentState;
                model.CreatedDateUtc = existingEntity.CreatedDateUtc;

                DbSet.Entry(model).State = EntityState.Modified;
                var result = await _dbContext.SaveChangesAsync() > 0;
                if (!result)
                    throw new DataAccessException($"Failed to update entity with key {model.Id}.", _logger);
                return new SaveResult() { Success = true, Id = model.Id };
            }
            catch (DbUpdateConcurrencyException concurrencyEx)
            {
                HandleException(nameof(UpdateAsync), $"Concurrency error while updating an entity of type {typeof(T).Name}, ID {model.Id}.", concurrencyEx);
                throw;
            }
            catch (Exception ex)
            {
                HandleException(nameof(UpdateAsync), $"Error occurred while updating an entity of type {typeof(T).Name}, ID {model.Id}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Updates the CurrentState of an entity.
        /// </summary>
        public async Task<bool> UpdateCurrentStateAsync(Guid entityId, Guid updaterId, int newValue = 0)
        {
            try
            {
                var entity = await DbSet.FindAsync(entityId);

                if (entity == null)
                    throw new NotFoundException($"Entity of type {typeof(T).Name} with ID {entityId} not found.", _logger);

                entity.CurrentState = newValue;
                entity.UpdatedDateUtc = DateTime.UtcNow;
                entity.UpdatedBy = updaterId;

                DbSet.Update(entity);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (DbUpdateException dbEx)
            {
                HandleException(nameof(UpdateCurrentStateAsync), $"Database update error while updating CurrentState for entity type {typeof(T).Name}, ID {entityId}.", dbEx);
                throw;
            }
            catch (Exception ex)
            {
                HandleException(nameof(UpdateCurrentStateAsync), $"Error occurred while updating CurrentState for entity type {typeof(T).Name}, ID {entityId}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Permanently deletes an entity by its ID.
        /// </summary>
        public async Task<bool> HardDeleteAsync(Guid id)
        {
            try
            {
                var entityToDelete = await DbSet.SingleOrDefaultAsync(e => e.Id == id);

                if (entityToDelete == null)
                    throw new DataAccessException($"Entity with key {id} not found.", _logger);

                DbSet.Remove(entityToDelete);

                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException concurrencyEx)
            {
                HandleException(nameof(HardDeleteAsync), $"Concurrency error while hard deleting an entity of type {typeof(T).Name}, ID {id}.", concurrencyEx);
                throw;
            }
            catch (Exception ex)
            {
                HandleException(nameof(HardDeleteAsync), $"Error occurred while hard deleting an entity of type {typeof(T).Name}, ID {id}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Saves all pending changes to the database.
        /// </summary>
        public async Task<bool> SaveChangeAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException concurrencyEx)
            {
                HandleException(nameof(SaveChangeAsync), $"Concurrency error while saving changes for entity type {typeof(T).Name}.", concurrencyEx);
                throw;
            }
            catch (Exception ex)
            {
                HandleException(nameof(SaveChangeAsync), $"Error occurred while saving changes for entity type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Logs and rethrows exceptions with detailed information.
        /// </summary>
        public async Task<bool> AddRangeAsync(IEnumerable<T> entities, Guid userId)
        {
            try
            {

                var utcNow = DateTime.UtcNow;

                foreach (var entity in entities)
                {
                    entity.Id = Guid.NewGuid();
                    entity.CreatedDateUtc = utcNow;
                    entity.CreatedBy = userId;
                    entity.CurrentState = 1;
                }

                DbSet.AddRange(entities);
                var changes = await _dbContext.SaveChangesAsync() > 0;

                return changes;
            }
            catch (Exception ex)
            {
                HandleException(nameof(AddRangeAsync), $"Error occurred while adding multiple entities of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Saves or updates an entity based on its key asynchronously.
        /// </summary>
        public async Task<bool> SaveAsyncWithoutId(T model, Guid userId)
        {
            try
            {
                if (model.Id == Guid.Empty)
                {
                    var createResult = await CreateAsync(model, userId);
                    return createResult.Success;
                }
                else
                {
                    var updateResult = await UpdateAsync(model, userId);
                    return updateResult.Success;
                }

            }
            catch (Exception ex)
            {
                HandleException(nameof(SaveAsync), $"Error occurred while saving or updating an entity of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Updates multiple entities with different values for the same field in a single SQL call.
        /// OPTIMIZED for scenarios where each entity gets a unique value (like unique serials).
        /// SECURE VERSION - Protected against SQL injection.
        /// </summary>
        public async Task<(bool Success, int UpdatedCount)> UpdateBulkFieldsAsync(
            Dictionary<Guid, Dictionary<string, object>> entityFieldValues,
            Guid updaterId)
        {
            try
            {
                if (!entityFieldValues?.Any() == true) return (false, 0);

                // SECURITY: Validate field names against entity properties
                var entityType = _dbContext.Model.FindEntityType(typeof(T));
                var validColumns = entityType.GetProperties()
                    .Select(p => p.GetColumnName())
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                var allFields = entityFieldValues.SelectMany(x => x.Value.Keys).Distinct();
                var invalidFields = allFields.Where(field => !validColumns.Contains(field)).ToList();
                if (invalidFields.Any())
                {
                    throw new ArgumentException($"Invalid field names: {string.Join(", ", invalidFields)}");
                }

                // SECURITY: Use parameterized queries and safe column name construction
                var tableName = entityType.GetTableName();
                var schemaName = entityType.GetSchema();
                var fullTableName = string.IsNullOrEmpty(schemaName) ?
                    $"[{tableName}]" :
                    $"[{schemaName}].[{tableName}]";

                // Build CASE statements for each field
                var setClauses = new List<string>();
                var parameters = new List<SqlParameter>();
                var paramIndex = 0;

                foreach (var fieldName in allFields)
                {
                    var caseStatements = new List<string>();

                    foreach (var entityValue in entityFieldValues)
                    {
                        if (entityValue.Value.TryGetValue(fieldName, out var value))
                        {
                            caseStatements.Add($"WHEN @id{paramIndex} THEN @value{paramIndex}");
                            parameters.Add(new SqlParameter($"@id{paramIndex}", entityValue.Key));
                            parameters.Add(new SqlParameter($"@value{paramIndex}", value ?? DBNull.Value));
                            paramIndex++;
                        }
                    }

                    if (caseStatements.Any())
                    {
                        var caseClause = string.Join(" ", caseStatements);
                        setClauses.Add($"[{fieldName}] = CASE [Id] {caseClause} END");
                    }
                }

                var entityIds = entityFieldValues.Keys.ToList();
                var idParams = string.Join(", ", entityIds.Select((_, i) => $"@entityId{i}"));

                // Add entity ID parameters for WHERE clause
                for (int i = 0; i < entityIds.Count; i++)
                {
                    parameters.Add(new SqlParameter($"@entityId{i}", entityIds[i]));
                }

                var setClause = string.Join(", ", setClauses);

                var sql = $@"
            UPDATE {fullTableName} 
            SET {setClause},
                [UpdatedDateUtc] = @UpdatedDateUtc,
                [UpdatedBy] = @UpdatedBy
            WHERE [Id] IN ({idParams})";

                // Add metadata parameters
                parameters.Add(new SqlParameter("@UpdatedDateUtc", DateTime.UtcNow));
                parameters.Add(new SqlParameter("@UpdatedBy", updaterId));

                var updatedCount = await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters.ToArray());
                return (updatedCount > 0, updatedCount);
            }
            catch (Exception ex)
            {
                HandleException(nameof(UpdateBulkFieldsAsync), $"Error occurred while bulk updating fields with different values for entities of type {typeof(T).Name}.", ex);
                throw;
            }
        }

        /// <summary>
        /// Simple bulk hard delete by IDs - no caching, no metadata complexity
        /// </summary>
        public async Task<(bool Success, int DeletedCount)> BulkHardDeleteByIdsAsync(IEnumerable<Guid> ids)
        {
            var idList = ids?.ToList();
            try
            {
                if (idList?.Any() != true) return (false, 0);

                // Get table name directly from EF Core
                var entityType = _dbContext.Model.FindEntityType(typeof(T));
                var tableName = entityType.GetTableName();
                var schemaName = entityType.GetSchema();
                var fullTableName = string.IsNullOrEmpty(schemaName) ? $"[{tableName}]" : $"[{schemaName}].[{tableName}]";

                // Process in chunks to avoid parameter limits
                const int chunkSize = 1000;
                var totalDeleted = 0;

                for (int i = 0; i < idList.Count; i += chunkSize)
                {
                    var chunk = idList.Skip(i).Take(chunkSize).ToList();
                    var chunkResult = await ExecuteHardDeleteChunk(chunk, fullTableName);
                    totalDeleted += chunkResult.DeletedCount;
                }

                return (totalDeleted > 0, totalDeleted);
            }
            catch (Exception ex)
            {
                HandleException(nameof(BulkHardDeleteByIdsAsync),
                    $"Error occurred while bulk hard deleting {idList?.Count} entities .", ex);
                throw;
            }
        }

        public IQueryable<T> GetWithInclude(Expression<Func<T, object>> include)
        {
            return _dbContext.Set<T>().Include(include);
        }

        /// <summary>
        /// Execute hard delete for a chunk of IDs
        /// </summary>
        private async Task<(bool Success, int DeletedCount)> ExecuteHardDeleteChunk(List<Guid> ids, string fullTableName)
        {
            var idParams = string.Join(", ", ids.Select((_, i) => $"@id{i}"));
            var sql = $"DELETE FROM {fullTableName} WHERE [Id] IN ({idParams})";

            var parameters = new SqlParameter[ids.Count];
            for (int i = 0; i < ids.Count; i++)
            {
                parameters[i] = new SqlParameter($"@id{i}", ids[i]);
            }

            var deletedCount = await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
            return (deletedCount >= 0, deletedCount);
        }

        private void HandleException(string methodName, string message, Exception ex)
        {
            // Log detailed message for analysis
            _logger.Error(ex, $"[{methodName}] {message}");

            // Throw exception with a general message for users
            throw new DataAccessException(message, ex, _logger);
        }
    }
}
