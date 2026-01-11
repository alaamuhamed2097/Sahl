using BL.Contracts.GeneralService;
using DAL.ApplicationContext;
using DAL.Contracts.Repositories;
using Domains.Entities.ECommerceSystem.Vendor;
using Domains.Identity;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace DAL.Repositories
{
    public class VendorRepository : TableRepository<TbVendor>, IVendorRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public VendorRepository(ApplicationDbContext dbContext, ICurrentUserService currentUserService, ILogger logger, UserManager<ApplicationUser> userManager)
            : base(dbContext, currentUserService, logger)
        {
            _userManager = userManager;
        }

        public async Task<(bool Success, IEnumerable<string> Errors)> RegisterVendorWithUserAsync(ApplicationUser user, string password, TbVendor vendor)
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
    }
}
