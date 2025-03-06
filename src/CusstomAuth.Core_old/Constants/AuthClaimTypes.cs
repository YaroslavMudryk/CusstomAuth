using System.Security.Claims;

namespace CusstomAuth.Core.Constants;

public static class AuthClaimTypes
{
    public const string UserId = ClaimTypes.NameIdentifier;
    public const string Role = ClaimTypes.Role;
    public const string Login = "login";
    public const string SessionId = "sessionId";
    public const string AuthenticationMethod = ClaimTypes.AuthenticationMethod;
    public const string Language = "language";
}
