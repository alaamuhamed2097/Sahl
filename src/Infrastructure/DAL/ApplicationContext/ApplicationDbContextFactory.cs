using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DAL.ApplicationContext
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            // Navigate to the Api project directory to find appsettings.json
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Presentation", "Api");
            
            // If the path doesn't exist (e.g., running from a different directory), try alternative paths
            if (!Directory.Exists(basePath))
            {
                basePath = Path.Combine(Directory.GetCurrentDirectory(), "src", "Presentation", "Api");
            }
            
            if (!Directory.Exists(basePath))
            {
                // Fallback to searching from solution root
                var currentDir = Directory.GetCurrentDirectory();
                while (!string.IsNullOrEmpty(currentDir))
                {
                    var apiPath = Path.Combine(currentDir, "src", "Presentation", "Api");
                    if (Directory.Exists(apiPath))
                    {
                        basePath = apiPath;
                        break;
                    }
                    currentDir = Directory.GetParent(currentDir)?.FullName;
                }
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
