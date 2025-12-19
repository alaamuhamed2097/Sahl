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
        protected readonly ApplicationDbContext _dbContext;
        protected readonly ILogger _logger;

        // Constants
        private const int BULK_OPERATION_CHUNK_SIZE = 1000;

        public TableRepository(ApplicationDbContext dbContext, ILogger logger) : base(dbContext, logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "Database context cannot be null.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger instance cannot be null.");
        }

        // ============================================
        // OVERRIDES - Filter Active Records Only
        // ============================================

        public override async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<T>()
                    .AsNoTracking()
                    .Where(e => !e.IsDeleted)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(GetAllAsync), $"Error occurred while retrieving all active entities of type {typeof(T).Name}.", ex);
                return Enumerable.Empty<T>();
            }
        }

        public override async Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>> predicate = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var query = _dbContext.Set<T>().AsNoTracking();

                if (predicate != null)
                {
                    query = query.Where(predicate);
                }

                query = query.Where(e => !e.IsDeleted);

                return await query.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(GetAsync), $"Error occurred while filtering active entities of type {typeof(T).Name}.", ex);
                return Enumerable.Empty<T>();
            }
        }

        public override async Task<PagedResult<T>> GetPageAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                ValidatePaginationParameters(pageNumber, pageSize);

                IQueryable<T> query = _dbContext.Set<T>().AsNoTracking();

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

                return new PagedResult<T>(data, totalCount);
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

        // ============================================
        // READ OPERATIONS - By ID
        // ============================================

        public async Task<T> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var data = await _dbContext.Set<T>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted, cancellationToken);

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
                return null;
            }
        }

        public async Task<T> FindByIdIncludingDeletedAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var data = await _dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);

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
                HandleException(nameof(FindByIdIncludingDeletedAsync),
                    $"Error occurred while finding an entity of type {typeof(T).Name} with ID {id}.", ex);
                return null;
            }
        }

        public IQueryable<T> GetWithInclude(Expression<Func<T, object>> include)
        {
            return _dbContext.Set<T>().Include(include);
        }

        public async Task<IEnumerable<T>> ListDeletedAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.Set<T>()
                    .AsNoTracking()
                    .Where(e => e.IsDeleted)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(ListDeletedAsync),
                    $"Error occurred while retrieving deleted entities of type {typeof(T).Name}.", ex);
                return Enumerable.Empty<T>();
            }
        }

        // ============================================
        // WRITE OPERATIONS - Single Entity
        // ============================================

        public async Task<SaveResult> SaveAsync(T model, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (model.Id == Guid.Empty)
                    return await CreateAsync(model, userId, cancellationToken);
                else
                    return await UpdateAsync(model, userId, cancellationToken);
            }
            catch (Exception ex)
            {
                HandleException(nameof(SaveAsync), $"Error occurred while saving or updating an entity of type {typeof(T).Name}.", ex);
                return new SaveResult { Success = false };
            }
        }

        public async Task<bool> SaveAsyncWithoutId(T model, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await SaveAsync(model, userId, cancellationToken);
                return result.Success;
            }
            catch (Exception ex)
            {
                HandleException(nameof(SaveAsyncWithoutId),
                    $"Error occurred while saving or updating an entity of type {typeof(T).Name}.", ex);
                return false;
            }
        }

        public async Task<SaveResult> CreateAsync(T model, Guid creatorId, CancellationToken cancellationToken = default)
        {
            try
            {
                var id = Guid.NewGuid();

                model.Id = id;
                model.CreatedDateUtc = DateTime.UtcNow;
                model.CreatedBy = creatorId;
                model.IsDeleted = false;

                await _dbContext.Set<T>().AddAsync(model, cancellationToken);
                var result = await _dbContext.SaveChangesAsync(cancellationToken) > 0;

                if (!result)
                    throw new DataAccessException($"Failed to create entity of type {typeof(T).Name}.", _logger);

                return new SaveResult { Success = true, Id = id };
            }
            catch (DbUpdateException dbEx)
            {
                HandleException(nameof(CreateAsync), $"Conflict error while creating an entity of type {typeof(T).Name}.", dbEx);
                return new SaveResult { Success = false };
            }
            catch (Exception ex)
            {
                HandleException(nameof(CreateAsync), $"Error occurred while creating an entity of type {typeof(T).Name}.", ex);
                return new SaveResult { Success = false };
            }
        }

        public async Task<SaveResult> UpdateAsync(T model, Guid updaterId, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingEntity = await _dbContext.Set<T>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == model.Id, cancellationToken);

                if (existingEntity == null)
                    throw new DataAccessException($"Entity with key {model.Id} not found.", _logger);

                model.UpdatedDateUtc = DateTime.UtcNow;
                model.UpdatedBy = updaterId;
                model.CreatedBy = existingEntity.CreatedBy;
                model.IsDeleted = existingEntity.IsDeleted;
                model.CreatedDateUtc = existingEntity.CreatedDateUtc;

                _dbContext.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                var result = await _dbContext.SaveChangesAsync(cancellationToken) > 0;

                if (!result)
                    throw new DataAccessException($"Failed to update entity with key {model.Id}.", _logger);

                return new SaveResult { Success = true, Id = model.Id };
            }
            catch (DbUpdateConcurrencyException concurrencyEx)
            {
                HandleException(nameof(UpdateAsync),
                    $"Concurrency error while updating an entity of type {typeof(T).Name}, ID {model.Id}.", concurrencyEx);
                return new SaveResult { Success = false };
            }
            catch (Exception ex)
            {
                HandleException(nameof(UpdateAsync),
                    $"Error occurred while updating an entity of type {typeof(T).Name}, ID {model.Id}.", ex);
                return new SaveResult { Success = false };
            }
        }

        public async Task<bool> UpdateCurrentStateAsync(
            Guid entityId,
            Guid updaterId,
            bool newState = true,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = await _dbContext.Set<T>().FindAsync(new object[] { entityId }, cancellationToken);

                if (entity == null)
                    throw new NotFoundException($"Entity of type {typeof(T).Name} with ID {entityId} not found.", _logger);

                entity.IsDeleted = newState;
                entity.UpdatedDateUtc = DateTime.UtcNow;
                entity.UpdatedBy = updaterId;

                _dbContext.Set<T>().Update(entity);
                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (DbUpdateException dbEx)
            {
                HandleException(nameof(UpdateCurrentStateAsync),
                    $"Database update error while updating CurrentState for entity type {typeof(T).Name}, ID {entityId}.", dbEx);
                return false;
            }
            catch (Exception ex)
            {
                HandleException(nameof(UpdateCurrentStateAsync),
                    $"Error occurred while updating CurrentState for entity type {typeof(T).Name}, ID {entityId}.", ex);
                return false;
            }
        }

        public async Task<bool> SoftDeleteAsync(Guid entityId, Guid updaterId, CancellationToken cancellationToken = default)
        {
            return await UpdateCurrentStateAsync(entityId, updaterId, true, cancellationToken);
        }

        public async Task<bool> RestoreAsync(Guid entityId, Guid updaterId, CancellationToken cancellationToken = default)
        {
            return await UpdateCurrentStateAsync(entityId, updaterId, false, cancellationToken);
        }

        public async Task<bool> HardDeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var entityToDelete = await _dbContext.Set<T>()
                    .SingleOrDefaultAsync(e => e.Id == id, cancellationToken);

                if (entityToDelete == null)
                    throw new DataAccessException($"Entity with key {id} not found.", _logger);

                _dbContext.Set<T>().Remove(entityToDelete);

                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (DbUpdateConcurrencyException concurrencyEx)
            {
                HandleException(nameof(HardDeleteAsync),
                    $"Concurrency error while hard deleting an entity of type {typeof(T).Name}, ID {id}.", concurrencyEx);
                return false;
            }
            catch (Exception ex)
            {
                HandleException(nameof(HardDeleteAsync),
                    $"Error occurred while hard deleting an entity of type {typeof(T).Name}, ID {id}.", ex);
                return false;
            }
        }

        public async Task<bool> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (DbUpdateConcurrencyException concurrencyEx)
            {
                HandleException(nameof(SaveChangeAsync),
                    $"Concurrency error while saving changes for entity type {typeof(T).Name}.", concurrencyEx);
                return false;
            }
            catch (Exception ex)
            {
                HandleException(nameof(SaveChangeAsync),
                    $"Error occurred while saving changes for entity type {typeof(T).Name}.", ex);
                return false;
            }
        }

        // ============================================
        // WRITE OPERATIONS - Bulk/Range
        // ============================================

        public async Task<bool> AddRangeAsync(
            IEnumerable<T> entities,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var utcNow = DateTime.UtcNow;

                foreach (var entity in entities)
                {
                    entity.Id = Guid.NewGuid();
                    entity.CreatedDateUtc = utcNow;
                    entity.CreatedBy = userId;
                    entity.IsDeleted = false;
                }

                _dbContext.Set<T>().AddRange(entities);
                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (Exception ex)
            {
                HandleException(nameof(AddRangeAsync),
                    $"Error occurred while adding multiple entities of type {typeof(T).Name}.", ex);
                return false;
            }
        }

        public async Task<bool> UpdateRangeAsync(
            IEnumerable<T> entities,
            Guid updaterId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var utcNow = DateTime.UtcNow;

                foreach (var entity in entities)
                {
                    entity.UpdatedDateUtc = utcNow;
                    entity.UpdatedBy = updaterId;
                }

                _dbContext.Set<T>().UpdateRange(entities);
                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (Exception ex)
            {
                HandleException(nameof(UpdateRangeAsync),
                    $"Error occurred while updating multiple entities of type {typeof(T).Name}.", ex);
                return false;
            }
        }

        public async Task<(bool Success, int DeletedCount)> SoftDeleteRangeAsync(
            IEnumerable<Guid> entityIds,
            Guid updaterId,
            CancellationToken cancellationToken = default)
        {
            return await BulkUpdateSingleFieldAsync(
                entityIds,
                nameof(BaseEntity.IsDeleted),
                true,
                updaterId,
                cancellationToken);
        }

        public async Task<(bool Success, int RestoredCount)> RestoreRangeAsync(
            IEnumerable<Guid> entityIds,
            Guid updaterId,
            CancellationToken cancellationToken = default)
        {
            return await BulkUpdateSingleFieldAsync(
                entityIds,
                nameof(BaseEntity.IsDeleted),
                false,
                updaterId,
                cancellationToken);
        }

        public async Task<(bool Success, int DeletedCount)> BulkHardDeleteByIdsAsync(
            IEnumerable<Guid> ids,
            CancellationToken cancellationToken = default)
        {
            var idList = ids?.ToList();

            try
            {
                if (idList?.Any() != true)
                    return (false, 0);

                using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

                try
                {
                    var entityType = _dbContext.Model.FindEntityType(typeof(T));
                    var tableName = EscapeIdentifier(entityType.GetTableName());
                    var schemaName = entityType.GetSchema();
                    var fullTableName = string.IsNullOrEmpty(schemaName)
                        ? $"[{tableName}]"
                        : $"[{EscapeIdentifier(schemaName)}].[{tableName}]";

                    var totalDeleted = 0;

                    for (int i = 0; i < idList.Count; i += BULK_OPERATION_CHUNK_SIZE)
                    {
                        var chunk = idList.Skip(i).Take(BULK_OPERATION_CHUNK_SIZE).ToList();
                        var chunkResult = await ExecuteHardDeleteChunk(chunk, fullTableName, cancellationToken);
                        totalDeleted += chunkResult.DeletedCount;
                    }

                    await transaction.CommitAsync(cancellationToken);
                    return (totalDeleted > 0, totalDeleted);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
            catch (Exception ex)
            {
                HandleException(nameof(BulkHardDeleteByIdsAsync),
                    $"Error occurred while bulk hard deleting {idList?.Count} entities.", ex);
                return (false, 0);
            }
        }

        // ============================================
        // ADVANCED BULK OPERATIONS
        // ============================================

        public async Task<(bool Success, int UpdatedCount)> UpdateBulkFieldsAsync(
            Dictionary<Guid, Dictionary<string, object>> entityFieldValues,
            Guid updaterId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (entityFieldValues?.Any() != true)
                    return (false, 0);

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

                var tableName = EscapeIdentifier(entityType.GetTableName());
                var schemaName = entityType.GetSchema();
                var fullTableName = string.IsNullOrEmpty(schemaName)
                    ? $"[{tableName}]"
                    : $"[{EscapeIdentifier(schemaName)}].[{tableName}]";

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
                        setClauses.Add($"[{EscapeIdentifier(fieldName)}] = CASE [Id] {caseClause} END");
                    }
                }

                var entityIds = entityFieldValues.Keys.ToList();
                var idParams = string.Join(", ", entityIds.Select((_, i) => $"@entityId{i}"));

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

                parameters.Add(new SqlParameter("@UpdatedDateUtc", DateTime.UtcNow));
                parameters.Add(new SqlParameter("@UpdatedBy", updaterId));

                var updatedCount = await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
                return (updatedCount > 0, updatedCount);
            }
            catch (Exception ex)
            {
                HandleException(nameof(UpdateBulkFieldsAsync),
                    $"Error occurred while bulk updating fields with different values for entities of type {typeof(T).Name}.", ex);
                return (false, 0);
            }
        }

        public async Task<(bool Success, int UpdatedCount)> BulkUpdateSingleFieldAsync(
            IEnumerable<Guid> entityIds,
            string fieldName,
            object newValue,
            Guid updaterId,
            CancellationToken cancellationToken = default)
        {
            var idList = entityIds?.ToList();

            try
            {
                if (idList?.Any() != true)
                    return (false, 0);

                if (string.IsNullOrWhiteSpace(fieldName))
                    throw new ArgumentException("Field name cannot be null or empty.", nameof(fieldName));

                var entityType = _dbContext.Model.FindEntityType(typeof(T));
                var validColumns = entityType.GetProperties()
                    .Select(p => p.GetColumnName())
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                if (!validColumns.Contains(fieldName))
                {
                    throw new ArgumentException($"Invalid field name: {fieldName}");
                }

                var tableName = EscapeIdentifier(entityType.GetTableName());
                var schemaName = entityType.GetSchema();
                var fullTableName = string.IsNullOrEmpty(schemaName)
                    ? $"[{tableName}]"
                    : $"[{EscapeIdentifier(schemaName)}].[{tableName}]";

                using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

                try
                {
                    var totalUpdated = 0;

                    for (int i = 0; i < idList.Count; i += BULK_OPERATION_CHUNK_SIZE)
                    {
                        var chunk = idList.Skip(i).Take(BULK_OPERATION_CHUNK_SIZE).ToList();

                        var parameters = new List<SqlParameter>
                        {
                            new SqlParameter("@FieldValue", newValue ?? DBNull.Value),
                            new SqlParameter("@UpdatedDateUtc", DateTime.UtcNow),
                            new SqlParameter("@UpdatedBy", updaterId)
                        };

                        for (int j = 0; j < chunk.Count; j++)
                        {
                            parameters.Add(new SqlParameter($"@id{j}", chunk[j]));
                        }

                        var idParams = string.Join(", ", chunk.Select((_, idx) => $"@id{idx}"));

                        var sql = $@"
                            UPDATE {fullTableName}
                            SET [{EscapeIdentifier(fieldName)}] = @FieldValue,
                                [UpdatedDateUtc] = @UpdatedDateUtc,
                                [UpdatedBy] = @UpdatedBy
                            WHERE [Id] IN ({idParams})";

                        var updatedCount = await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
                        totalUpdated += updatedCount;
                    }

                    await transaction.CommitAsync(cancellationToken);
                    return (totalUpdated > 0, totalUpdated);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
            catch (Exception ex)
            {
                HandleException(nameof(BulkUpdateSingleFieldAsync),
                    $"Error occurred while bulk updating field '{fieldName}' for {idList?.Count} entities.", ex);
                return (false, 0);
            }
        }

        // ============================================
        // PRIVATE HELPER METHODS
        // ============================================

        private async Task<(bool Success, int DeletedCount)> ExecuteHardDeleteChunk(
            List<Guid> ids,
            string fullTableName,
            CancellationToken cancellationToken = default)
        {
            var idParams = string.Join(", ", ids.Select((_, i) => $"@id{i}"));
            var sql = $"DELETE FROM {fullTableName} WHERE [Id] IN ({idParams})";

            var parameters = new SqlParameter[ids.Count];
            for (int i = 0; i < ids.Count; i++)
            {
                parameters[i] = new SqlParameter($"@id{i}", ids[i]);
            }

            var deletedCount = await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
            return (deletedCount >= 0, deletedCount);
        }

        private string EscapeIdentifier(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException("Identifier cannot be null or empty.", nameof(identifier));

            return identifier.Replace("]", "]]");
        }
    }
}