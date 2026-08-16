using InventoryManagement.Api.Domain.Enums;

namespace InventoryManagement.Api.Domain.Entities;

/// <summary>
/// An immutable audit record of every stock movement (in, out, or adjustment).
/// Stock transactions should never be deleted or updated — only new records are added.
/// </summary>
public class StockTransaction
{
    public Guid Id { get; set; }

    // ── Foreign keys ──────────────────────────────────────────────────────────
    public Guid ProductId { get; set; }

    /// <summary>Id of the User who created this transaction record.</summary>
    public Guid CreatedBy { get; set; }

    // ── Transaction data ──────────────────────────────────────────────────────

    /// <summary>
    /// Direction and purpose of the stock movement.
    /// Stored as integer. See <see cref="TransactionType"/>.
    /// Required.
    /// </summary>
    public TransactionType Type { get; set; }

    /// <summary>
    /// Number of units moved. Always stored as a positive integer.
    /// The <see cref="Type"/> determines whether this increases or decreases stock.
    /// Required. Must be > 0.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Unit price at the time of transaction (purchase price for StockIn, sale price for StockOut).
    /// Stored separately from Product.UnitPrice as prices change over time.
    /// Precision (18, 2). Required.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Free-text explanation of why this transaction occurred.
    /// Optional. Max 500 chars.
    /// Examples: "Received PO-12345", "Damaged in transit", "Cycle count adjustment".
    /// </summary>
    public string? Reason { get; set; }

    // ── Audit ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// UTC timestamp. Set once on creation; never updated (transactions are immutable).
    /// </summary>
    public DateTime CreatedAt { get; set; }

    // ── Navigation ────────────────────────────────────────────────────────────
    public Product Product { get; set; } = null!;
    public User CreatedByUser { get; set; } = null!;
}
