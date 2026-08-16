using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Api.Data;

/// <summary>
/// Application database context for the Inventory Management System.
/// Uses Supabase PostgreSQL as the backing store via Npgsql.
/// Entities (Product, Category, etc.) will be added in subsequent steps.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Entity configurations will be registered here as entities are added.
        // Example: modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
