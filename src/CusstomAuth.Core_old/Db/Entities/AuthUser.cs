using CusstomAuth.Core.Db.Audits;

namespace CusstomAuth.Core.Db.Entities;

public class AuthUser : AuditableSoftDeletedBaseModelWithIdentity<int>
{
    public AuthUser()
    {
        ConfirmationToken = null;
        ConfirmationSentAt = null;
        RecoveryToken = null;
        RecoverySentAt = null;
        Email = null;
        EmailChangeToken = null;
        EmailChangeSentAt = null;
        Phone = null;
        PhoneChangeToken = null;
        PhoneChangeSentAt = null;
        IsSuperAdmin = false;
        LastSignIn = null;
        CanBeBanned = true;
        CanBeBlocked = true;
        FailedAccessCount = 0;
        BannedUntil = null;
    }

    public AuthUser(string login, string passwordHash) : this()
    {
        Login = login;
        PasswordHash = passwordHash;
    }

    public AuthUser(string firstName, string lastName, string login, string passwordHash) : this()
    {
        FirstName = firstName;
        LastName = lastName;
        Login = login;
        PasswordHash = passwordHash;
        Email = login;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ImageUrl { get; set; }

    public string Login { get; set; }
    public string PasswordHash { get; set; }

    public bool IsConfirmed { get; set; }
    public string ConfirmationToken { get; set; }
    public DateTime? ConfirmationSentAt { get; set; }

    public string RecoveryToken { get; set; }
    public DateTime? RecoverySentAt { get; set; }

    public string Email { get; set; }
    public string EmailChangeToken { get; set; }
    public DateTime? EmailChangeSentAt { get; set; }
    public string Phone { get; set; }
    public string PhoneChangeToken { get; set; }
    public DateTime? PhoneChangeSentAt { get; set; }

    public bool IsSuperAdmin { get; set; }
    public DateTime? LastSignIn { get; set; }
    public bool CanBeBanned { get; set; }
    public int FailedAccessCount { get; set; }
    public DateTime? BannedUntil { get; set; }

    public bool CanBeBlocked { get; set; }
    public DateTime? BlockedUntil { get; set; }
    public int FailedLoginAttempts { get; set; }

    public bool Mfa { get; set; }
    public string MfaSecretKey { get; set; }

    public List<AuthPassword> Passwords { get; set; }
    public List<AuthBlock> Blocks { get; set; }
    public List<AuthMfa> Mfas { get; set; }
    public List<AuthConfirm> Confirms { get; set; }
    public List<AuthContact> Contacts { get; set; }
    public List<AuthUserRole> UserRoles { get; set; }
    public List<AuthExternalLogin> ExternalLogins { get; set; }
    public List<AuthQr> Qrs { get; set; }
    public List<AuthLoginAttempt> LoginAttempts { get; set; }
    public List<AuthSession> Sessions { get; set; }

    public bool IsBlocked(DateTime dateTime)
    {
        if (!CanBeBlocked)
            return false;

        bool isLocked;
        if (!BlockedUntil.HasValue)
            isLocked = false;
        else
        {
            if (BlockedUntil.Value > dateTime)
                isLocked = true;
            else
            {
                BlockedUntil = null;
                isLocked = false;
            }
        }
        return isLocked;
    }
}
