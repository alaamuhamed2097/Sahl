using DAL.ApplicationContext;
using Domains.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Api.Extensions.Infrastructure
{
    /// <summary>
    /// Extension methods for configuring database and Entity Framework Core.
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Adds Entity Framework Core with SQL Server and Identity configuration.
        /// </summary>
        /// <param name="services">The IServiceCollection instance.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configure Entity Framework
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
                // Suppress pending model changes warning during runtime migrations.
                options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
            });

            // Add Identity services
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Configure password requirements
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                // Configure lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}
