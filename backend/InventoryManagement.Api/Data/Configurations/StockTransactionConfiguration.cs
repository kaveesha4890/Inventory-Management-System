using InventoryManagement.Api.Domain.Entities;
using InventoryManagement.Api.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Api.Data.Configurations;

public class StockTransactionConfiguration : IEntityTypeConfiguration<StockTransaction>
{
    public void Configure(EntityTypeBuilder<StockTransaction> builder)
    {
        builder.ToTable("stock_transactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        // Store enum as its integer value for compact storage and fast filtering.
        // Avoids string comparison overhead vs. storing as varchar.
        builder.Property(t => t.Type)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnName("type");

        builder.Property(t => t.Quantity)
            .IsRequired();
        // Application layer enforces Quantity > 0.

        builder.Property(t => t.UnitPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(t => t.Reason)
            .HasMaxLength(500); // Optional audit note

        builder.Property(t => t.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("now()");
        // No UpdatedAt — transactions are intentionally immutable.

        // ── Indexes for common query patterns ─────────────────────────────────
        builder.HasIndex(t => t.ProductId)
            .HasDatabaseName("ix_stock_transactions_product_id");

        builder.HasIndex(t => t.CreatedBy)
            .HasDatabaseName("ix_stock_transactions_created_by");

        builder.HasIndex(t => t.CreatedAt)
            .HasDatabaseName("ix_stock_transactions_created_at");
        // Supports efficient date-range filtering for reports and audit logs.

        // ── Relationship: StockTransaction → Product ──────────────────────────
        builder.HasOne(t => t.Product)
            .WithMany(p => p.StockTransactions)
            .HasForeignKey(t => t.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        // Products with transactions should never be hard-deleted (use soft-delete in future).

        // ── Relationship: StockTransaction → User (CreatedBy) ─────────────────
        builder.HasOne(t => t.CreatedByUser)
            .WithMany(u => u.StockTransactions)
            .HasForeignKey(t => t.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
        // Preserve transaction history even if the creating user is deactivated.
    }
}
