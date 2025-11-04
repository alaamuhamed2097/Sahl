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
            await SeedSettingAsync(context);
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

        private static async Task SeedSettingAsync(ApplicationDbContext context)
        {
            // Check if settings already exist
            if (await context.Set<TbSetting>().AnyAsync())
            {
                return; // Settings already seeded
            }

            var defaultSetting = new TbSetting
            {
                // Contact Information
                Email = "info@yourcompany.com",
                Phone = "1234567890",
                PhoneCode = "+20",
                Address = "123 Main Street, Cairo, Egypt",

                // Social Media Links
                FacebookUrl = "https://facebook.com/yourcompany",
                InstagramUrl = "https://instagram.com/yourcompany",
                TwitterUrl = "https://twitter.com/yourcompany",
                LinkedInUrl = "https://linkedin.com/company/yourcompany",

                // WhatsApp
                WhatsAppNumber = "1234567890",
                WhatsAppCode = "+20",

                // Banner
                MainBannerPath = null,

                // Pricing & Tax Settings
                ShippingAmount = 0m,
                OrderTaxPercentage = 14m,
                OrderExtraCost = 0m,

                // SEO Fields (من BaseSeo)
                SEOTitle = "Your Company - Online Store",
                SEODescription = "Welcome to our online store offering the best products and services",
                SEOMetaTags = "ecommerce, online store, shopping, egypt",

                // Base Entity Fields
                CreatedBy = Guid.Empty,
                CreatedDateUtc = DateTime.UtcNow,
            };

            await context.Set<TbSetting>().AddAsync(defaultSetting);
            await context.SaveChangesAsync();
        }
    }
}
