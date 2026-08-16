namespace InventoryManagement.Api.Domain.Entities;

/// <summary>
/// Junction entity for the Product ↔ Supplier many-to-many relationship.
/// Uses a composite primary key (ProductId + SupplierId).
/// </summary>
public class ProductSupplier
{
    public Guid ProductId { get; set; }
    public Guid SupplierId { get; set; }

    // ── Navigation ────────────────────────────────────────────────────────────
    public Product Product { get; set; } = null!;
    public Supplier Supplier { get; set; } = null!;
}
