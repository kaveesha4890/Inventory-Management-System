namespace InventoryManagement.Api.Domain.Entities;

/// <summary>
/// A company or individual that supplies products.
/// </summary>
public class Supplier
{
    public Guid Id { get; set; }

    /// <summary>Required. Max 200 chars.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Optional. Max 255 chars.</summary>
    public string? Email { get; set; }

    /// <summary>Optional. Max 20 chars (supports international formats).</summary>
    public string? Phone { get; set; }

    /// <summary>Optional. Max 500 chars (multi-line address stored as single field).</summary>
    public string? Address { get; set; }

    // ── Audit ─────────────────────────────────────────────────────────────────
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // ── Navigation ────────────────────────────────────────────────────────────
    public ICollection<ProductSupplier> ProductSuppliers { get; set; } = new List<ProductSupplier>();
}
