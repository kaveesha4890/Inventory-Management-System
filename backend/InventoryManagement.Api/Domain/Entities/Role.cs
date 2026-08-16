namespace InventoryManagement.Api.Domain.Entities;

/// <summary>
/// Lookup table for user roles (e.g. Admin, Manager, Staff).
/// </summary>
public class Role
{
    public int Id { get; set; }

    /// <summary>Unique role name. Max 50 chars. Required.</summary>
    public string Name { get; set; } = string.Empty;

    // ── Navigation ────────────────────────────────────────────────────────────
    public ICollection<User> Users { get; set; } = new List<User>();
}
