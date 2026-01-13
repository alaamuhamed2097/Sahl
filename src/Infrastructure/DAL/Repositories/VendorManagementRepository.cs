using BL.Contracts.GeneralService;
using Common.Enumerations.User;
using Common.Filters;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories;
using DAL.Exceptions;
using DAL.Models;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public class VendorManagementRepository : TableRepository<TbVendor>, IVendorManagementRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public VendorManagementRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService, ILogger logger, UserManager<ApplicationUser> userManager)
            : base(dbContext, currentUserService, logger)
        {
            _userManager = userManager;
        }

        public async Task<PagedResult<TbVendor>> GetPageAsync(BaseSearchCriteriaModel criteriaModel, CancellationToken cancellationToken)
        {
            try
            {
                ValidatePaginationParameters(criteriaModel.PageNumber, criteriaModel.PageSize);

                // Base filter for active entities
                Expression<Func<TbVendor, bool>> filter = x => !x.IsDeleted;

                // Apply search term if provided
                if (!string.IsNullOrWhiteSpace(criteriaModel.SearchTerm))
                {
                    string searchTerm = criteriaModel.SearchTerm.Trim().ToLower();
                    filter = x => !x.IsDeleted &&
                                  (x.StoreName != null && x.StoreName.ToLower().Contains(searchTerm) ||
                                   x.User.FirstName != null && x.User.FirstName.ToLower().Contains(searchTerm) ||
                                   x.User.LastName != null && x.User.LastName.ToLower().Contains(searchTerm)
                                  );
                }

                // Create ordering function based on SortBy and SortDirection
                Func<IQueryable<TbVendor>, IOrderedQueryable<TbVendor>> orderBy = null;

                if (!string.IsNullOrWhiteSpace(criteriaModel.SortBy))
                {
                    var sortBy = criteriaModel.SortBy.ToLower();
                    var isDescending = criteriaModel.SortDirection?.ToLower() == "desc";

                    orderBy = query =>
                    {
                        return sortBy switch
                        {
                            "storename" => isDescending ? query.OrderByDescending(x => x.StoreName) : query.OrderBy(x => x.StoreName),
                            "contactname" => isDescending ? query.OrderByDescending(x => x.User.FirstName) : query.OrderBy(x => x.User.FirstName),
                            "registrationdate" => isDescending ? query.OrderByDescending(x => x.User.CreatedDateUtc) : query.OrderBy(x => x.User.CreatedDateUtc),
                            _ => query.OrderBy(x => x.StoreName)
                        };
                    };
                }

                IQueryable<TbVendor> query = _dbContext.Set<TbVendor>()
                    .AsNoTracking()
                    .Include(v => v.User);

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                int totalCount = await query.CountAsync(cancellationToken);

                query = query.Skip((criteriaModel.PageNumber - 1) * criteriaModel.PageSize).Take(criteriaModel.PageSize);

                var data = await query.ToListAsync(cancellationToken);

                return new PagedResult<TbVendor>(data, totalCount);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred in {nameof(GetPageAsync)} method for entity type {typeof(TbVendor).Name}.");
                throw new DataAccessException(
                    $"Error occurred in {nameof(GetPageAsync)} method for entity type {typeof(TbVendor).Name}.",
                    ex,
                    _logger
                );
            }
        }

        public async Task<(bool Success, IEnumerable<string> Errors)> RegisterVendorWithUserAsync(
            ApplicationUser user, string password, TbVendor vendor)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    // No need to rollback as nothing else happened in DB yet via this transaction context 
                    // (UserManager uses its own SaveChanges but usually shares context if same instance injection, but here we can be safe).
                    // Actually if UserManager shares context, it might have saved partial data? 
                    // UserManager usually calls SaveChangesAsync internally. If it shares _dbContext, the Transaction covers it.
                    // If it succeeded, we proceed. If failed, it should have auto-rolled back its own scoped changes or didn't save.
                    // But explicitly rolling back our transaction is safer if partially done.
                    return (false, result.Errors.Select(e => e.Code));
                }

                var addedToRole = await _userManager.AddToRoleAsync(user, "Vendor");
                if (!addedToRole.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return (false, addedToRole.Errors.Select(e => e.Code));
                }

                vendor.UserId = user.Id;
                // Ensure CreatedBy is set if not already
                if (vendor.CreatedBy == Guid.Empty && Guid.TryParse(user.Id, out var userIdGuid))
                {
                    vendor.CreatedBy = userIdGuid;
                }

                await _dbContext.Set<TbVendor>().AddAsync(vendor);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return (true, Enumerable.Empty<string>());
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.Error(ex, "Error registering vendor transaction");
                return (false, new List<string> { ex.Message });
            }
        }

        public async Task<(bool Success, IEnumerable<string> Errors)> UpdateVendorWithUserAsync(
            ApplicationUser user, TbVendor vendor, string? oldFrontImagePath = null, string? oldBackImagePath = null)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // Update ApplicationUser
                var userUpdateResult = await _userManager.UpdateAsync(user);
                if (!userUpdateResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return (false, userUpdateResult.Errors.Select(e => e.Code));
                }

                // Update Vendor
                _dbContext.Set<TbVendor>().Update(vendor);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                // Delete old images after successful commit (outside transaction)
                if (!string.IsNullOrEmpty(oldFrontImagePath))
                {
                    await DeleteImageAsync(oldFrontImagePath);
                }
                if (!string.IsNullOrEmpty(oldBackImagePath))
                {
                    await DeleteImageAsync(oldBackImagePath);
                }

                return (true, Enumerable.Empty<string>());
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.Error(ex, "Error updating vendor transaction");
                return (false, new List<string> { ex.Message });
            }
        }

        public async Task<TbVendor> FindByVendorIdAsync(Guid vendorId, CancellationToken cancellationToken)
        {
            try
            {
                var data = await _dbContext.Set<TbVendor>()
                    .AsNoTracking()
                    .Include(v => v.User)
                    .FirstOrDefaultAsync(e => e.Id == vendorId && !e.IsDeleted, cancellationToken);

                if (data == null)
                    throw new NotFoundException($"Entity of type {typeof(TbVendor).Name} with ID {vendorId} not found.", _logger);

                return data;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                HandleException(nameof(FindByIdAsync), $"Error occurred while finding an entity of type {typeof(TbVendor).Name} with ID {vendorId}.", ex);
                return null;
            }
        }

        public async Task<bool> UpdateUserStateAsync(Guid vendorId, UserStateType newType, CancellationToken cancellationToken)
        {
            try
            {
                var vendor = await _dbContext.Set<TbVendor>()
                    .Include(v => v.User)
                    .FirstOrDefaultAsync(e => e.Id == vendorId, cancellationToken);

                if (vendor == null)
                    throw new NotFoundException($"Entity of type {typeof(TbVendor).Name} with ID {vendorId} not found.", _logger);

                vendor.User.UserState = newType;

                _dbContext.Set<ApplicationUser>().Update(vendor.User);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                HandleException(nameof(FindByIdAsync), $"Error occurred while finding an entity of type {typeof(TbVendor).Name} with ID {vendorId}.", ex);
                return false;
            }
        }

        private async Task DeleteImageAsync(string imagePath)
        {
            try
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), imagePath);
                if (File.Exists(fullPath))
                {
                    await Task.Run(() => File.Delete(fullPath));
                }
            }
            catch (Exception ex)
            {
                _logger.Warning(ex, $"Failed to delete old image: {imagePath}");
            }
        }
    }
}
