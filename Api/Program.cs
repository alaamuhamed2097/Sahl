using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure OpenAPI
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new OpenApiInfo
        {
            Title = "Your API",
            Version = "v1",
            Description = "API Documentation",
            Contact = new OpenApiContact
            {
                Name = "Your Name",
                Email = "your.email@example.com"
            }
        };
        return Task.CompletedTask;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // يولد الـ OpenAPI JSON على /openapi/v1.json

    // 🔥 Add Swagger UI (Optional but recommended)
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Your API v1");
        options.RoutePrefix = "swagger"; // Access via /swagger
    });

    // 🔥 Endpoint لتصدير OpenAPI JSON للـ api-specs folder
    app.MapGet("/openapi/export", async (HttpContext context) =>
    {
        // Get OpenAPI document
        var openApiJson = await context.Request.HttpContext
            .RequestServices
            .GetRequiredService<IHttpClientFactory>()
            .CreateClient()
            .GetStringAsync($"{context.Request.Scheme}://{context.Request.Host}/openapi/v1.json");

        // Save to api-specs folder
        var solutionRoot = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.FullName;
        var apiSpecsPath = Path.Combine(solutionRoot!, "api-specs");
        Directory.CreateDirectory(apiSpecsPath);

        var filePath = Path.Combine(apiSpecsPath, "swagger.json");
        await File.WriteAllTextAsync(filePath, openApiJson);

        return Results.Ok(new
        {
            message = "OpenAPI JSON exported successfully",
            path = filePath,
            url = $"{context.Request.Scheme}://{context.Request.Host}/openapi/v1.json"
        });
    }).ExcludeFromDescription(); // Hide from OpenAPI docs
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();