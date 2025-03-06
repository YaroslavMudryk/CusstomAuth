using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Db;
using CusstomAuth.Core.Db.Entities;
using CusstomAuth.Core.Features.SignIn.Dtos;
using CusstomAuth.Core.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CusstomAuth.Core.Features.SignIn.Services;

public interface ITokenService
{
    AuthRefreshToken GetRefreshToken(Guid sessionId);
    Task<string> GenerateAccessTokenAsync(UserTokenDto userToken);
}

public class TokenService(
    AuthDbContext db,
    TimeProvider dateTimeProvider,
    IOptions<CusstomAuthOptions> options) : ITokenService
{
    public async Task<string> GenerateAccessTokenAsync(UserTokenDto userToken)
    {
        userToken.Lang = userToken.Lang.ToLower();

        var user = userToken.User ?? await db.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Id == userToken.UserId);

        var session = userToken.Session ?? await db.Sessions.AsNoTracking().FirstOrDefaultAsync(s => s.Id == userToken.SessionId);

        var userRoles = await db.UserRoles.AsNoTracking().Include(s => s.Role).Where(s => s.UserId == userToken.UserId).Select(s => s.Role).ToListAsync();

        var userRolesIds = userRoles.Select(s => s.Id);

        var claims = new List<Claim>
        {
            new(AuthClaimTypes.UserId, user.Id.ToString()),
            new(AuthClaimTypes.Login, user.Login),
            new(AuthClaimTypes.SessionId, session.Id.ToString()),
            new(AuthClaimTypes.AuthenticationMethod, session.Type.ToString()),
            new(AuthClaimTypes.Language, session.Language)
        };

        foreach (var role in userRoles)
        {
            claims.Add(new Claim(AuthClaimTypes.Role, role.Name));
        }

        var claimsFromRole = await db.RoleClaims.AsNoTracking().Include(s => s.Claim).Where(s => userRolesIds.Contains(s.RoleId)).Select(s => s.Claim).ToListAsync();

        var claimsFromApp = await db.AppClaims.AsNoTracking().Include(s => s.Claim).Where(s => s.AppId == session.App.Id).Select(s => s.Claim).ToListAsync();

        var claimsForToken = GetUniqClaims([claimsFromRole, claimsFromApp]);

        foreach (var claim in claimsForToken)
        {
            claims.Add(new Claim(claim.Type, claim.Value));
        }

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

        var utcNow = dateTimeProvider.GetUtcNow().UtcDateTime;
        var expiredAt = utcNow.AddMinutes(options.Value.Token.LifetimeInMinutes);
        var jwt = new JwtSecurityToken(
            issuer: options.Value.Token.Issuer,
            audience: options.Value.Token.Audience,
            notBefore: utcNow,
            claims: claimsIdentity.Claims,
            expires: expiredAt,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.Value.Token.SecretKey)), SecurityAlgorithms.HmacSha256));

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }

    private static List<AuthClaim> GetUniqClaims(IEnumerable<IEnumerable<AuthClaim>> source)
    {
        var claims = new List<AuthClaim>();

        foreach (var enumerable in source)
        {
            foreach (var claim in enumerable)
            {
                if (!claims.Any(s => s.Type == claim.Type && s.Value == claim.Value))
                    claims.Add(claim);
            }
        }

        return claims;
    }

    public AuthRefreshToken GetRefreshToken(Guid sessionId)
    {
        return new AuthRefreshToken
        {
            ExpiredAt = dateTimeProvider.GetUtcNow().UtcDateTime.AddMinutes(options.Value.RefreshToken.LifetimeInMinutes),
            SessionId = sessionId,
            Token = Guid.CreateVersion7().ToString(),
            TokenUsedAt = null
        };
    }
}
