using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using InventoryManagement.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// ── Services ──────────────────────────────────────────────────────────────────

builder.Services.AddControllers();

// Register Swashbuckle Swagger generator
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Inventory Management API",
        Version = "v1",
        Description = "ASP.NET Core Web API for the Inventory & Stock Management System"
    });
});

// ── Database (EF Core + Npgsql / Supabase PostgreSQL) ─────────────────────────
//
// Connection string resolution order (highest wins):
//   1. Environment variable  : ConnectionStrings__DefaultConnection
//   2. User Secrets (dev)    : dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=..."
//   3. appsettings.json      : placeholder — intentionally empty, never put real creds here
//
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    // Log a clear warning; the app will still start but DB endpoints will fail gracefully.
    Console.WriteLine(
        "[WARNING] ConnectionStrings:DefaultConnection is not configured. " +
        "Database features will be unavailable. " +
        "See backend/connection-string-guide.txt for setup instructions.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (!string.IsNullOrWhiteSpace(connectionString))
    {
        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
            // Retry transient failures (e.g., cold-start on Supabase free tier)
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorCodesToAdd: null);
        });
    }
    else
    {
        // Register with no provider so DI resolution still works;
        // actual DB calls will throw and be caught by the health checker.
        options.UseNpgsql(string.Empty);
    }
});

// Register the lightweight DB health checker
builder.Services.AddScoped<DatabaseHealthChecker>();

// ── Build ──────────────────────────────────────────────────────────────────────

var app = builder.Build();

// ── Middleware pipeline ────────────────────────────────────────────────────────

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory Management API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ── Built-in health endpoints ──────────────────────────────────────────────────

// Basic liveness check — always returns 200 (no DB involved)
app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    timestamp = DateTime.UtcNow
})).WithTags("Health");

// Database connectivity check — tests actual Supabase connection
app.MapGet("/health/db", async (DatabaseHealthChecker checker, CancellationToken ct) =>
{
    var result = await checker.CheckAsync(ct);
    return result.IsHealthy
        ? Results.Ok(result)
        : Results.Json(result, statusCode: 503);
}).WithTags("Health");

app.Run();


