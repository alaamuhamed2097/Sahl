using Common.Enumerations;
using DAL.ResultModels;
using Domains.Entities.Base;
using System.Linq.Expressions;

namespace DAL.Contracts.Repositories
{

    /// <summary>
    /// Extended repository interface for entities with BaseEntity properties
    /// Includes CRUD operations and state management
    /// </summary>
    /// <typeparam name="T">Entity type inheriting from BaseEntity</typeparam>
    public interface ITableRepository<T> : IRepository<T> where T : BaseEntity
    {
        // ============================================
        // READ OPERATIONS - By ID
        // ============================================

        /// <summary>
        /// Finds an active entity by its ID
        /// </summary>
        Task<T> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds an entity by its ID including soft-deleted records
        /// </summary>
        Task<T> FindByIdIncludingDeletedAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets queryable with include for complex queries
        /// </summary>
        IQueryable<T> GetWithInclude(Expression<Func<T, object>> include);

        /// <summary>
        /// Lists all soft-deleted entities
        /// </summary>
        Task<IEnumerable<T>> ListDeletedAsync(CancellationToken cancellationToken = default);

        // ============================================
        // WRITE OPERATIONS - Single Entity
        // ============================================

        /// <summary>
        /// Saves (creates or updates) an entity and returns the ID
        /// </summary>
        Task<SaveResult> SaveAsync(T model, Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves entity without returning the ID
        /// </summary>
        Task<bool> SaveAsyncWithoutId(T model, Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new entity
        /// </summary>
        Task<SaveResult> CreateAsync(T model, Guid creatorId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        Task<SaveResult> UpdateAsync(T model, Guid updaterId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the CurrentState of an entity (soft delete/restore)
        /// </summary>
        Task<bool> UpdateCurrentStateAsync(
            Guid entityId,
            Guid updaterId,
            EntityState newState = EntityState.Deleted,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Soft deletes an entity by setting CurrentState to 0
        /// </summary>
        Task<bool> SoftDeleteAsync(Guid entityId, Guid updaterId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Restores a soft-deleted entity by setting CurrentState to 1
        /// </summary>
        Task<bool> RestoreAsync(Guid entityId, Guid updaterId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Permanently deletes an entity
        /// </summary>
        Task<bool> HardDeleteAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves all pending changes to the database
        /// </summary>
        Task<bool> SaveChangeAsync(CancellationToken cancellationToken = default);

        // ============================================
        // WRITE OPERATIONS - Bulk/Range
        // ============================================

        /// <summary>
        /// Adds multiple entities in a single operation
        /// </summary>
        Task<bool> AddRangeAsync(
            IEnumerable<T> entities,
            Guid userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates multiple entities in a single operation
        /// </summary>
        Task<bool> UpdateRangeAsync(
            IEnumerable<T> entities,
            Guid updaterId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Soft deletes multiple entities
        /// </summary>
        Task<(bool Success, int DeletedCount)> SoftDeleteRangeAsync(
            IEnumerable<Guid> entityIds,
            Guid updaterId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Restores multiple soft-deleted entities
        /// </summary>
        Task<(bool Success, int RestoredCount)> RestoreRangeAsync(
            IEnumerable<Guid> entityIds,
            Guid updaterId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Permanently deletes multiple entities by IDs with transaction support
        /// </summary>
        Task<(bool Success, int DeletedCount)> BulkHardDeleteByIdsAsync(
            IEnumerable<Guid> ids,
            CancellationToken cancellationToken = default);

        // ============================================
        // ADVANCED BULK OPERATIONS
        // ============================================

        /// <summary>
        /// Updates multiple entities with different field values in a single SQL operation
        /// OPTIMIZED: Each entity can have unique values for multiple fields
        /// SECURE: Protected against SQL injection
        /// </summary>
        /// <param name="entityFieldValues">
        /// Dictionary where:
        /// - Key: Entity ID (Guid)
        /// - Value: Dictionary of field names and their new values
        /// Example: { EntityId1: { "Status": "Active", "Priority": 1 }, EntityId2: { "Status": "Pending", "Priority": 2 } }
        /// </param>
        Task<(bool Success, int UpdatedCount)> UpdateBulkFieldsAsync(
            Dictionary<Guid, Dictionary<string, object>> entityFieldValues,
            Guid updaterId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a single field to the same value for multiple entities
        /// More efficient than UpdateBulkFieldsAsync when all entities get the same value
        /// </summary>
        /// <param name="entityIds">IDs of entities to update</param>
        /// <param name="fieldName">Name of the field to update</param>
        /// <param name="newValue">New value for all entities</param>
        Task<(bool Success, int UpdatedCount)> BulkUpdateSingleFieldAsync(
            IEnumerable<Guid> entityIds,
            string fieldName,
            object newValue,
            Guid updaterId,
            CancellationToken cancellationToken = default);
    }
}
