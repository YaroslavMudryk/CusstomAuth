using System.Security.Claims;

namespace CusstomAuth.Core.Options;

public class ClaimsIdentityOptions
{
    public string RoleClaimType { get; set; } = ClaimTypes.Role;
    public string UserNameClaimType { get; set; } = ClaimTypes.Name;
    public string UserIdClaimType { get; set; } = ClaimTypes.NameIdentifier;
    public string EmailClaimType { get; set; } = ClaimTypes.Email;
    public string SecurityStampClaimType { get; set; } = "caSecurityStamp";
}
