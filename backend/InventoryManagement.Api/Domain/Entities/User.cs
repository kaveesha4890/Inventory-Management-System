namespace InventoryManagement.Api.Domain.Entities;

/// <summary>
/// Application user. Credentials (PasswordHash) are stored here;
/// JWT / session handling will be added in the authentication increment.
/// </summary>
public class User
{
    public Guid Id { get; set; }

    /// <summary>Required. Max 100 chars.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Required. Max 100 chars.</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>Required. Max 255 chars. Unique index enforced via configuration.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// BCrypt / Argon2 hash of the user's password.
    /// Never store plain-text passwords.
    /// Required.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    // ── Foreign key ───────────────────────────────────────────────────────────
    public int RoleId { get; set; }

    // ── Audit ─────────────────────────────────────────────────────────────────
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // ── Navigation ────────────────────────────────────────────────────────────
    public Role Role { get; set; } = null!;
    public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
}
