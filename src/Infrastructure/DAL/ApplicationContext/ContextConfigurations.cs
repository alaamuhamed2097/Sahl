using Common.Enumerations.User;
using Domains.Entities.Setting;
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
            try
            {
                Console.WriteLine("[ContextConfigurations.Configure] Starting...");

                if (modelBuilder == null)
                {
                    Console.WriteLine("[ContextConfigurations.Configure] ERROR: modelBuilder is null!");
                    throw new ArgumentNullException(nameof(modelBuilder));
                }

                // Use the non-generic Identity role types (string keys) to match ApplicationUser which uses string Id
                modelBuilder.Entity<ApplicationUser>()?.ToTable("Users");
                modelBuilder.Entity<IdentityRole>()?.ToTable("Roles");
                modelBuilder.Entity<IdentityUserRole<string>>()?.ToTable("UserRoles");
                modelBuilder.Entity<IdentityUserClaim<string>>()?.ToTable("UserClaims");
                modelBuilder.Entity<IdentityUserLogin<string>>()?.ToTable("UserLogins");
                modelBuilder.Entity<IdentityUserToken<string>>()?.ToTable("UserTokens");
                modelBuilder.Entity<IdentityRoleClaim<string>>()?.ToTable("RoleClaims");

                Console.WriteLine("[ContextConfigurations.Configure] Completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ContextConfigurations.Configure] ERROR: {ex.Message}");
                Console.WriteLine($"[ContextConfigurations.Configure] Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public static async Task SeedDataAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            try
            {
                Console.WriteLine("[SeedDataAsync] Starting...");

                if (context == null)
                    throw new ArgumentNullException(nameof(context));
                if (userManager == null)
                    throw new ArgumentNullException(nameof(userManager));
                if (roleManager == null)
                    throw new ArgumentNullException(nameof(roleManager));

                await SeedRolesAsync(roleManager);
                await SeedUserAsync(userManager);
                await SeedSettingAsync(context);

                Console.WriteLine("[SeedDataAsync] Completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SeedDataAsync] ERROR: {ex.Message}");
                Console.WriteLine($"[SeedDataAsync] Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            try
            {
                Console.WriteLine("[SeedRolesAsync] Starting...");

                // Ensure roles exist
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    var adminRole = new IdentityRole("Admin") { Id = Guid.NewGuid().ToString() };
                    await roleManager.CreateAsync(adminRole);
                    Console.WriteLine("[SeedRolesAsync] Created Admin role");
                }

                if (!await roleManager.RoleExistsAsync("Vendor"))
                {
                    var vendorRole = new IdentityRole("Vendor") { Id = Guid.NewGuid().ToString() };
                    await roleManager.CreateAsync(vendorRole);
                    Console.WriteLine("[SeedRolesAsync] Created Vendor role");
                }

                if (!await roleManager.RoleExistsAsync("Customer"))
                {
                    var customerRole = new IdentityRole("Customer") { Id = Guid.NewGuid().ToString() };
                    await roleManager.CreateAsync(customerRole);
                    Console.WriteLine("[SeedRolesAsync] Created Customer role");
                }

                Console.WriteLine("[SeedRolesAsync] Completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SeedRolesAsync] ERROR: {ex.Message}");
                throw;
            }
        }

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
        {
            try
            {
                Console.WriteLine("[SeedUserAsync] Starting...");

                // Ensure admin user exists
                var adminEmail = seedAdminEmail;
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        Id = Guid.NewGuid().ToString(),
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
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                        Console.WriteLine("[SeedUserAsync] Created Admin user");
                    }
                }

                // Ensure Vendor user exists
                var VendorEmail = seedVendorEmail;
                var VendorUser = await userManager.FindByEmailAsync(VendorEmail);
                if (VendorUser == null)
                {
                    VendorUser = new ApplicationUser
                    {
                        Id = Guid.NewGuid().ToString(),
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
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(VendorUser, "Vendor");
                        Console.WriteLine("[SeedUserAsync] Created Vendor user");
                    }
                }

                Console.WriteLine("[SeedUserAsync] Completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SeedUserAsync] ERROR: {ex.Message}");
                throw;
            }
        }

        private static async Task SeedSettingAsync(ApplicationDbContext context)
        {
            try
            {
                Console.WriteLine("[SeedSettingAsync] Starting...");

                // Check if settings already exist
                if (await context.Set<TbGeneralSettings>().AnyAsync())
                {
                    Console.WriteLine("[SeedSettingAsync] Settings already exist, skipping...");
                    return; // Settings already seeded
                }

                var defaultSetting = new TbGeneralSettings
                {
                    // Contact Information
                    Email = "info@yourcompany.com",
                    Phone = "1234567890",
                    Address = "123 Main Street, Cairo, Egypt",

                    // Social Media Links
                    FacebookUrl = "https://facebook.com/yourcompany",
                    InstagramUrl = "https://instagram.com/yourcompany",
                    TwitterUrl = "https://twitter.com/yourcompany",
                    LinkedInUrl = "https://linkedin.com/company/yourcompany",

                    // WhatsApp
                    WhatsAppNumber = "1234567890",

                    // SEO Fields (من BaseSeo)
                    SEOTitle = "Your Company - Online Store",
                    SEODescription = "Welcome to our online store offering the best products and services",
                    SEOMetaTags = "ecommerce, online store, shopping, egypt",

                    // Base Entity Fields
                    CreatedBy = Guid.Empty,
                    CreatedDateUtc = DateTime.UtcNow,
                };

                await context.Set<TbGeneralSettings>().AddAsync(defaultSetting);
                await context.SaveChangesAsync();

                Console.WriteLine("[SeedSettingAsync] Completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SeedSettingAsync] ERROR: {ex.Message}");
                throw;
            }
        }
    }
}