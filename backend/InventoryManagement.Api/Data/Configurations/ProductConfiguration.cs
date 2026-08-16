using InventoryManagement.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Api.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.SKU)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Description)
            .HasMaxLength(1000); // Optional

        builder.Property(p => p.UnitPrice)
            .IsRequired()
            .HasPrecision(18, 2); // Supports up to 9,999,999,999,999,999.99

        builder.Property(p => p.ReorderLevel)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("now()");

        builder.Property(p => p.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("now()");

        // ── Unique index on SKU ────────────────────────────────────────────────
        builder.HasIndex(p => p.SKU)
            .IsUnique()
            .HasDatabaseName("ix_products_sku");

        // ── Non-unique index on CategoryId (common filter column) ─────────────
        builder.HasIndex(p => p.CategoryId)
            .HasDatabaseName("ix_products_category_id");

        // ── Relationship: Product belongs to one Category ─────────────────────
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict); // Cannot delete a category that has products
    }
}
