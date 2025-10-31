using Common.Enumerations.User;
using Domains.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.ApplicationContext
{
    public class ContextConfigurations
    {
        private static readonly string seedAdminEmail = "admin@gmail.com";
        private static readonly string seedVendorEmail = "vendor@gmail.com";

        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        }

        public static async Task SeedDataAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            await SeedRolesAsync(roleManager);
            await SeedUserAsync(userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            // Ensure roles exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("Vendor"))
            {
                await roleManager.CreateAsync(new IdentityRole("Vendor"));
            }
        }

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
        {
            // Ensure admin user exists
            var adminEmail = seedAdminEmail;
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var id = Guid.NewGuid().ToString();
                adminUser = new ApplicationUser
                {
                    Id = id,
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "Eladmin",
                    ProfileImagePath = "uploads/Images/ProfileImages/default.png",
                    PhoneNumber = "1234560",
                    PhoneCode = "+20",
                    CreatedBy = Guid.Empty,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastLoginDate = DateTime.UtcNow,
                    UserState = UserStateType.Active
                };
                var result = await userManager.CreateAsync(adminUser, "admin123456");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Ensure Vendor user exists
            var VendorEmail = seedVendorEmail;
            var VendorUser = await userManager.FindByEmailAsync(VendorEmail);
            if (VendorUser == null)
            {
                var id = Guid.NewGuid().ToString();
                VendorUser = new ApplicationUser
                {
                    Id = id,
                    UserName = VendorEmail,
                    Email = VendorEmail,
                    EmailConfirmed = true,
                    FirstName = "Vendor",
                    LastName = "ElVendor",
                    ProfileImagePath = "uploads/Images/ProfileImages/default.png",
                    PhoneNumber = "1234560",
                    PhoneCode = "+20",
                    CreatedBy = Guid.Empty,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastLoginDate = null,
                    UserState = UserStateType.Active
                };
                var result = await userManager.CreateAsync(VendorUser, "Vendor123456");
                await userManager.AddToRoleAsync(VendorUser, "Vendor");
            }
        }
    }
}
