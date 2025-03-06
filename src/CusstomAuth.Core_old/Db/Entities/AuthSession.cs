using CusstomAuth.Core.Db.Audits;
using CusstomAuth.Core.Db.Entities.Internal;
using CusstomAuth.Core.Features.SignIn.Dtos;
using CusstomAuth.Core.Mappers;

namespace CusstomAuth.Core.Db.Entities;

public class AuthSession : AuditableSoftDeletedBaseModelWithIdentity<Guid>
{
    public string PublicId { get; set; }
    public AppModel App { get; set; }
    public ClientModel Client { get; set; }
    public LocationModel Location { get; set; }
    public bool ViaMFA { get; set; }
    public SessionType Type { get; set; }
    public string Language { get; set; }
    public DateTime? ExpiredAt { get; set; }
    public SessionStatus Status { get; set; }
    public DateTime? DeactivatedAt { get; set; }
    public Guid? DeactivatedBySessionId { get; set; }
    public DateTime? LastActivity { get; set; }
    public int UserId { get; set; }
    public AuthUser User { get; set; }
    public Guid DeviceId { get; set; }
    public AuthDevice Device { get; set; }
    public List<AuthRefreshToken> RefreshTokens { get; set; }

    public bool IsFinished()
    {
        return Status is SessionStatus.Completed or SessionStatus.Terminated;
    }

    public static AuthSession New(AppModel appInfo, SignInDto signInDto, LocationModel location, AuthUser user)
    {
        return new AuthSession
        {
            Id = Guid.CreateVersion7(),
            PublicId = Guid.NewGuid().ToString("N"),
            App = appInfo,
            Client = signInDto.Device.MapToClient(),
            Location = location,
            UserId = user.Id,
            Type = SessionType.Password,
            ViaMFA = user.Mfa,
            Status = SessionStatus.Pending,
            Language = signInDto.Lang
        };
    }
}

public enum SessionStatus
{
    Pending = 1,
    Active,
    Completed,
    Terminated
}

public enum SessionType
{
    Password = 1,
    ExternalService,
    Qr
}
