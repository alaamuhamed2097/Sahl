using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DAL.ApplicationContext
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            try
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

                Console.WriteLine($"[Design-Time] Environment: {environment}");
                Console.WriteLine($"[Design-Time] Current Directory: {Directory.GetCurrentDirectory()}");

                // Try a few candidate base paths to locate appsettings.json
                string? basePath = FindBasePath();

                // Ensure we have a non-null, existing base path
                if (string.IsNullOrWhiteSpace(basePath) || !Directory.Exists(basePath))
                {
                    basePath = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
                }

                basePath = Path.GetFullPath(basePath);

                Console.WriteLine($"[Design-Time] Using base path: {basePath}");

                var configurationBuilder = new ConfigurationBuilder();

                // Set base path safely (some environments can throw here)
                try
                {
                    configurationBuilder.SetBasePath(basePath);
                }
                catch (Exception setBaseEx)
                {
                    Console.WriteLine($"[Design-Time] Warning: SetBasePath failed: {setBaseEx.Message}. Falling back to AppContext.BaseDirectory.");
                    configurationBuilder.SetBasePath(AppContext.BaseDirectory);
                }

                configurationBuilder
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: false)
                    .AddEnvironmentVariables();

                IConfiguration configuration = configurationBuilder.Build();

                // Try multiple sources for the connection string
                var connectionString = configuration.GetConnectionString("DefaultConnection")
                                       ?? Environment.GetEnvironmentVariable("DefaultConnection")
                                       ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

                // As a fallback, try loading appsettings.json directly from the chosen basePath
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    try
                    {
                        var appsettingsPath = Path.Combine(basePath, "appsettings.json");
                        if (File.Exists(appsettingsPath))
                        {
                            var directConfig = new ConfigurationBuilder()
                                .AddJsonFile(appsettingsPath, optional: false, reloadOnChange: false)
                                .AddEnvironmentVariables()
                                .Build();

                            connectionString = directConfig.GetConnectionString("DefaultConnection");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Design-Time] Warning: Failed to read appsettings.json directly: {ex.Message}");
                    }
                }

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException(
                        "Connection string 'DefaultConnection' not found.\n\n" +
                        "Troubleshooting steps:\n" +
                        "1. Ensure appsettings.json contains 'ConnectionStrings:DefaultConnection'\n" +
                        "2. When running from solution root, use:\n" +
                        "   dotnet ef migrations add MigrationName --project src/Infrastructure/DAL/DAL.csproj --startup-project src/Presentation/Api/Api.csproj\n" +
                        "3. Or set environment variable: ConnectionStrings__DefaultConnection\n" +
                        $"4. Current directory: {Directory.GetCurrentDirectory()}\n" +
                        $"5. Base path used: {basePath}");
                }

                Console.WriteLine($"[Design-Time] Connection string found: {connectionString.Substring(0, Math.Min(50, connectionString.Length))}...");

                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(connectionString);

                // Instantiate the context and force model building so we can capture model creation exceptions now
                var context = new ApplicationDbContext(optionsBuilder.Options);

                try
                {
                    // Accessing Model will trigger OnModelCreating and model validation
                    var _ = context.Model;
                }
                catch (Exception modelEx)
                {
                    Console.Error.WriteLine("[Design-Time] ERROR: Failed while building the EF model at design time.");
                    Console.Error.WriteLine(modelEx.ToString());
                    throw; // rethrow so EF tooling sees the failure
                }

                return context;
            }
            catch (Exception ex)
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "(null)";
                var cwd = Directory.GetCurrentDirectory();
                var message = $"Failed to create ApplicationDbContext at design time.\n" +
                              $"Current Directory: '{cwd}'\n" +
                              $"Environment: '{env}'\n" +
                              $"Error: {ex.Message}\n\n" +
                              "See inner exception for details.";

                Console.Error.WriteLine(message);
                Console.Error.WriteLine(ex.ToString());

                throw new InvalidOperationException(message, ex);
            }
        }

        private string FindBasePath()
        {
            var currentDir = Directory.GetCurrentDirectory();

            var candidates = new[]
            {
                // When running from DAL project folder
                Path.Combine(currentDir, "..", "..", "Presentation", "Api"),
                // When running from solution root
                Path.Combine(currentDir, "src", "Presentation", "Api"),
                Path.Combine(currentDir, "Presentation", "Api"),
                // When running from Api project folder
                currentDir,
                // One level up (if in bin/Debug)
                Path.Combine(currentDir, ".."),
                // Two levels up
                Path.Combine(currentDir, "..", ".."),
            };

            foreach (var candidate in candidates)
            {
                try
                {
                    if (Directory.Exists(candidate))
                    {
                        var appsettingsPath = Path.Combine(candidate, "appsettings.json");
                        if (File.Exists(appsettingsPath))
                        {
                            Console.WriteLine($"[Design-Time] Found appsettings.json at: {appsettingsPath}");
                            return candidate;
                        }
                    }
                }
                catch
                {
                    // ignore permission issues and continue
                }
            }

            // Last resort: try to find Api project folder by walking up the tree
            var dir = new DirectoryInfo(currentDir);
            while (dir != null)
            {
                var possible = Path.Combine(dir.FullName, "src", "Presentation", "Api");
                if (Directory.Exists(possible) && File.Exists(Path.Combine(possible, "appsettings.json")))
                    return possible;

                dir = dir.Parent;
            }

            // Last resort: use current directory
            Console.WriteLine($"[Design-Time] WARNING: appsettings.json not found in any candidate path. Using current directory.");
            return currentDir;
        }
    }
}