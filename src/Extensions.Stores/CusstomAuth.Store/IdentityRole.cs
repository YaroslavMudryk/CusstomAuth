namespace CusstomAuth;

public class IdentityRole : IdentityRole<int>
{

}

public class IdentityRole<TKey> : IdentityBaseModel<TKey> where TKey : IEquatable<TKey>
{
    public IdentityRole() { }

    public IdentityRole(string roleName) : this()
    {
        Name = roleName;
    }

    public virtual string Name { get; set; }
    public virtual bool IsDefault { get; set; }
    public virtual string NameNormalized { get; set; }
    public virtual List<IdentityUserRole> UserRoles { get; set; }
    public virtual List<IdentityRoleClaim> RoleClaims { get; set; }

    public virtual string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    public override string ToString()
    {
        return Name ?? string.Empty;
    }
}
