namespace InventoryManagement.Api.Domain.Entities;

/// <summary>
/// A stockable item in the inventory.
/// </summary>
public class Product
{
    public Guid Id { get; set; }

    /// <summary>Required. Max 200 chars.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Stock Keeping Unit — unique product identifier used in warehousing.
    /// Required. Max 50 chars. Unique index enforced via configuration.
    /// </summary>
    public string SKU { get; set; } = string.Empty;

    /// <summary>Optional. Max 1000 chars.</summary>
    public string? Description { get; set; }

    /// <summary>
    /// Selling / reference price per unit.
    /// Precision (18, 2) — supports values up to 9,999,999,999,999,999.99.
    /// Required. Must be >= 0.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Minimum stock level before a reorder alert is triggered.
    /// Required. Must be >= 0.
    /// </summary>
    public int ReorderLevel { get; set; }

    // ── Foreign key ───────────────────────────────────────────────────────────
    public int CategoryId { get; set; }

    // ── Audit ─────────────────────────────────────────────────────────────────
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // ── Navigation ────────────────────────────────────────────────────────────
    public Category Category { get; set; } = null!;
    public ICollection<ProductSupplier> ProductSuppliers { get; set; } = new List<ProductSupplier>();
    public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
}
