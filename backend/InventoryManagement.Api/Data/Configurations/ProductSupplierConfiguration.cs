using InventoryManagement.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Api.Data.Configurations;

public class ProductSupplierConfiguration : IEntityTypeConfiguration<ProductSupplier>
{
    public void Configure(EntityTypeBuilder<ProductSupplier> builder)
    {
        builder.ToTable("product_suppliers");

        // ── Composite primary key ──────────────────────────────────────────────
        // The combination of ProductId + SupplierId must be unique,
        // which naturally enforces the many-to-many constraint.
        builder.HasKey(ps => new { ps.ProductId, ps.SupplierId });

        // ── Relationship: ProductSupplier → Product ────────────────────────────
        builder.HasOne(ps => ps.Product)
            .WithMany(p => p.ProductSuppliers)
            .HasForeignKey(ps => ps.ProductId)
            .OnDelete(DeleteBehavior.Cascade); // Removing a product clears its supplier links

        // ── Relationship: ProductSupplier → Supplier ───────────────────────────
        builder.HasOne(ps => ps.Supplier)
            .WithMany(s => s.ProductSuppliers)
            .HasForeignKey(ps => ps.SupplierId)
            .OnDelete(DeleteBehavior.Cascade); // Removing a supplier clears its product links
    }
}
