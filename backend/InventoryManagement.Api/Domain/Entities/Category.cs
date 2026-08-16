namespace InventoryManagement.Api.Domain.Entities;

/// <summary>
/// Product category (e.g. Electronics, Clothing, Raw Materials).
/// </summary>
public class Category
{
    public int Id { get; set; }

    /// <summary>Required. Max 100 chars. Unique.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Optional. Max 500 chars.</summary>
    public string? Description { get; set; }

    // ── Audit ─────────────────────────────────────────────────────────────────
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // ── Navigation ────────────────────────────────────────────────────────────
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
