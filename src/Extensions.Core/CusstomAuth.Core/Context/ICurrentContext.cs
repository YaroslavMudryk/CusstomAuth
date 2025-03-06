using CusstomAuth.Core.Constants;
using Microsoft.AspNetCore.Http;

namespace CusstomAuth;

public interface ICurrentContext
{
    string GetIp();
    bool IsAdmin();
    bool IsAuthenticated();
    CurrentUser User { get; }
}

public class HttpCurrentContext(IHttpContextAccessor httpContextAccessor) : ICurrentContext
{
    public CurrentUser User => new()
    {
        Id = !IsAuthenticated() ? 0 : Convert.ToInt32(httpContextAccessor.HttpContext.User.Claims.First(s => s.Type == AuthClaimTypes.UserId).Value),
        Roles = !IsAuthenticated() ? [] : httpContextAccessor.HttpContext.User.Claims.Where(s => s.Type == AuthClaimTypes.Role).Select(s => s.Value),
        SessionId = !IsAuthenticated() ? Guid.Empty : Guid.Parse(httpContextAccessor.HttpContext.User.Claims.First(s => s.Type == AuthClaimTypes.SessionId).Value),
        Login = !IsAuthenticated() ? null : httpContextAccessor.HttpContext.User.Claims.First(s => s.Type == AuthClaimTypes.Login).Value,
        Language = !IsAuthenticated() ? null : httpContextAccessor.HttpContext.User.Claims.First(s => s.Type == AuthClaimTypes.Language).Value,
        AuthenticationMethod = !IsAuthenticated() ? null : httpContextAccessor.HttpContext.User.Claims.First(s => s.Type == AuthClaimTypes.AuthenticationMethod).Value
    };

    public string By => IsAuthenticated() ? User.Id.ToString() : "0";

    public string GetBearerToken()
    {
        var bearerWord = "Bearer ";
        var bearerToken = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        if (bearerToken.StartsWith(bearerWord, StringComparison.OrdinalIgnoreCase))
        {
            return bearerToken.Substring(bearerWord.Length).Trim();
        }
        return bearerToken;
    }

    public string GetIp()
    {
        return httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
    }

    public bool IsAdmin()
    {
        if (!IsAuthenticated())
            return false;
        return User.Roles.Any(s => s.Contains(DefaultsRoles.Administrator));
    }

    public bool IsAuthenticated() => httpContextAccessor.HttpContext.User.Identity!.IsAuthenticated;
}
