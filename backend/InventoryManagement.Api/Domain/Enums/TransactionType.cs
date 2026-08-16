namespace InventoryManagement.Api.Domain.Enums;

/// <summary>
/// Represents the direction and purpose of a stock movement.
/// Stored as an integer column in PostgreSQL via EF Core value conversion.
/// </summary>
public enum TransactionType
{
    /// <summary>Stock received into inventory (purchase, return from customer, etc.)</summary>
    StockIn = 1,

    /// <summary>Stock removed from inventory (sale, write-off, etc.)</summary>
    StockOut = 2,

    /// <summary>Manual correction to reconcile physical vs system stock count.</summary>
    Adjustment = 3
}
