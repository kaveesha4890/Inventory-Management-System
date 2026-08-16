using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Api.Data;

/// <summary>
/// Verifies that the API can open a real connection to the PostgreSQL database.
/// Used exclusively by the /health/db endpoint — not for production health-check infrastructure.
/// </summary>
public class DatabaseHealthChecker
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<DatabaseHealthChecker> _logger;

    public DatabaseHealthChecker(AppDbContext dbContext, ILogger<DatabaseHealthChecker> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Attempts to open a connection and run a trivial query against the database.
    /// Returns a structured result suitable for the health endpoint response.
    /// </summary>
    public async Task<DatabaseHealthResult> CheckAsync(CancellationToken cancellationToken = default)
    {
        var started = DateTime.UtcNow;
        try
        {
            // CanConnect opens a real TCP connection without requiring any tables to exist.
            var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);

            if (!canConnect)
            {
                _logger.LogWarning("Database connectivity check: unable to connect.");
                return DatabaseHealthResult.Unhealthy("Unable to open a connection to the database.", started);
            }

            // Run a lightweight round-trip query to confirm the server responds correctly.
            var serverVersion = await _dbContext.Database
                .SqlQuery<string>($"SELECT version()")
                .FirstOrDefaultAsync(cancellationToken);

            _logger.LogInformation("Database connectivity check passed. Server: {ServerVersion}", serverVersion);
            return DatabaseHealthResult.Healthy(serverVersion ?? "unknown", started);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database connectivity check failed with an exception.");
            // Intentionally do NOT include ex.Message in the response to avoid leaking
            // connection string details or internal database errors to callers.
            return DatabaseHealthResult.Unhealthy("Database connection check failed. See server logs for details.", started);
        }
    }
}

/// <summary>Structured result of a database health check.</summary>
public sealed record DatabaseHealthResult(
    bool IsHealthy,
    string Message,
    string? ServerVersion,
    DateTime CheckedAt,
    double ElapsedMs)
{
    public static DatabaseHealthResult Healthy(string serverVersion, DateTime started) =>
        new(true, "Database connection successful.", serverVersion, started,
            (DateTime.UtcNow - started).TotalMilliseconds);

    public static DatabaseHealthResult Unhealthy(string message, DateTime started) =>
        new(false, message, null, started,
            (DateTime.UtcNow - started).TotalMilliseconds);
}
