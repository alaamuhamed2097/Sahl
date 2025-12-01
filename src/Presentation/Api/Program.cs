using Api.Extensions;
using BL.GeneralService.Notification;
using DAL.ApplicationContext;
using Domains.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Logging (Serilog)
builder.Services.AddSerilogConfiguration(builder.Configuration);
builder.Host.AddSerilogHost();

// Configure Database & Identity
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// Configure Authentication (JWT)
builder.Services.AddJwtAuthentication(builder.Configuration);

// Configure AutoMapper
builder.Services.AddAutoMapperConfiguration();

// Configure Repository Services
builder.Services.AddRepositoryServices();

// Configure Business Services
builder.Services.AddCmsServices();
builder.Services.AddNotificationServices();
builder.Services.AddUserManagementServices();
builder.Services.AddLocationServices();
builder.Services.AddGeneralServices();

// Configure E-Commerce Services
builder.Services.AddECommerceConfiguration(builder.Configuration);

// Configure New Services
builder.Services.AddWarehouseAndInventoryServices();
builder.Services.AddContentManagementServices();
builder.Services.AddEnhancedNotificationServices();

// Configure Infrastructure Services
builder.Services.AddInfrastructureServices();

// Configure Localization
builder.Services.AddLocalizationConfiguration();

// Configure MVC & Compression
builder.Services.AddMvcConfiguration();
builder.Services.AddCompressionConfiguration();

// Configure CORS
builder.Services.AddCorsConfiguration(builder.Environment);

// Configure Swagger
builder.Services.AddSwaggerConfiguration();
builder.Services.AddHttpClient();

// Configure Hangfire
//builder.Services.AddHangfireConfiguration(builder.Configuration);

var app = builder.Build();

// Configure middleware pipeline
app.UseSwagger(options =>
{
    options.RouteTemplate = "openapi/{documentName}.json";
});

//if (app.Environment.IsDevelopment())
//{
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Sahl API v1");
    options.RoutePrefix = "swagger";

    app.MapGet("/", () => Results.Redirect("/swagger"))
       .ExcludeFromDescription();

    options.DefaultModelsExpandDepth(-1);
    options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    options.EnableDeepLinking();
    options.DisplayRequestDuration();
    options.EnableFilter();
    options.ShowExtensions();
    options.EnableValidator();
});

app.MapGet("/openapi/export", async context =>
{
    var httpClientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();

    var baseUrl = $"{context.Request.Scheme}://{context.Request.Host}";
    var openApiUrl = $"{baseUrl}/openapi/v1.json";

    var openApiJson = await httpClient.GetStringAsync(openApiUrl);

    var solutionRoot = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.FullName
                       ?? Directory.GetCurrentDirectory();
    var specsPath = Path.Combine(solutionRoot, "api-specs");
    Directory.CreateDirectory(specsPath);

    var filePath = Path.Combine(specsPath, "swagger.json");
    await File.WriteAllTextAsync(filePath, openApiJson);

    //return Results.Ok(new
    //{
    //    message = "OpenAPI JSON exported successfully",
    //    path = filePath,
    //    url = openApiUrl
    //});
}).ExcludeFromDescription();
//}
//else
//{
//    app.UseSwaggerUI(c =>
//    {
//        app.MapGet("/", () => Results.Redirect("/swagger"));
//        c.DefaultModelsExpandDepth(-1);
//        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
//        c.EnableDeepLinking();
//        c.DisplayRequestDuration();
//        c.EnableFilter();
//    });
//}

// Middleware order
app.UseStaticFiles();
app.UseResponseCompression();
app.UseHttpsRedirection();

// Apply CORS policy
app.UseCors("AllowAll");

// Configure localization
app.UseLocalizationConfiguration();

app.UseRouting();

// Authentication/Authorization
app.UseAuthentication();
app.UseAuthorization();

// SignalR hub mapping
app.MapHub<NotificationHub>("/notificationHub");

app.MapControllers();

// Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var dbContext = services.GetRequiredService<ApplicationDbContext>();


    // Apply migrations
    await dbContext.Database.MigrateAsync();

    // Seed data
    // await ContextConfigurations.SeedDataAsync(dbContext, userManager, roleManager);
}

// Add debugging middleware for Swagger
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/swagger"))
    {
        Console.WriteLine($"Swagger request: {context.Request.Path}");
    }
    await next();
});

// Use the ClientIP middleware
app.UseMiddleware<Api.Middleware.ClientIPMiddleware>();

// run
app.Run();