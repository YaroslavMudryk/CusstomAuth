namespace CusstomAuth;

public class IdentityUser : IdentityUser<int>
{
    public IdentityUser(string login, string passwordHash) : base()
    {
        Login = login;
        PasswordHash = passwordHash;
    }

    public IdentityUser(string firstName, string lastName, string login, string passwordHash) : base()
    {
        FirstName = firstName;
        LastName = lastName;
        Login = login;
        PasswordHash = passwordHash;
        Email = login;
    }
}

public class IdentityUser<TKey> : IdentityBaseModel<TKey> where TKey : IEquatable<TKey>
{
    public IdentityUser()
    {
        ConfirmationToken = null!;
        ConfirmationSentAt = null;
        RecoveryToken = null!;
        RecoverySentAt = null;
        Email = null!;
        EmailChangeToken = null!;
        EmailChangeSentAt = null;
        Phone = null!;
        PhoneChangeToken = null!;
        PhoneChangeSentAt = null;
        IsSuperAdmin = false;
        LastSignIn = null;
        CanBeBanned = true;
        CanBeBlocked = true;
        FailedAccessCount = 0;
        BannedUntil = null;
    }

    public IdentityUser(string login, string passwordHash) : this()
    {
        Login = login;
        PasswordHash = passwordHash;
    }

    public IdentityUser(string firstName, string lastName, string login, string passwordHash) : this()
    {
        FirstName = firstName;
        LastName = lastName;
        Login = login;
        PasswordHash = passwordHash;
        Email = login;
    }

    public virtual string FirstName { get; set; } = default!;
    public virtual string LastName { get; set; } = default!;
    public virtual string ImageUrl { get; set; } = default!;
    public virtual string Login { get; set; } = default!;
    public virtual string PasswordHash { get; set; } = default!;
    public virtual bool IsConfirmed { get; set; }
    public virtual string ConfirmationToken { get; set; } = default!;
    public virtual DateTime? ConfirmationSentAt { get; set; }
    public virtual string RecoveryToken { get; set; } = default!;
    public virtual DateTime? RecoverySentAt { get; set; }
    public virtual string Email { get; set; } = default!;
    public virtual string EmailChangeToken { get; set; } = default!;
    public virtual DateTime? EmailChangeSentAt { get; set; }
    public virtual string Phone { get; set; } = default!;
    public virtual string PhoneChangeToken { get; set; } = default!;
    public virtual DateTime? PhoneChangeSentAt { get; set; }
    public virtual bool IsSuperAdmin { get; set; }
    public virtual DateTime? LastSignIn { get; set; }
    public virtual bool CanBeBanned { get; set; }
    public virtual int FailedAccessCount { get; set; }
    public virtual DateTime? BannedUntil { get; set; }
    public virtual bool CanBeBlocked { get; set; }
    public virtual DateTime? BlockedUntil { get; set; }
    public virtual int FailedLoginAttempts { get; set; }
    public virtual bool Mfa { get; set; }
    public virtual string MfaSecretKey { get; set; } = default!;
    public virtual List<IdentityPassword> Passwords { get; set; } = default!;
    public virtual List<IdentityBlock> Blocks { get; set; } = default!;
    public virtual List<IdentityMfa> Mfas { get; set; } = default!;
    public virtual List<IdentityConfirm> Confirms { get; set; } = default!;
    public virtual List<IdentityContact> Contacts { get; set; } = default!;
    public virtual List<IdentityUserRole> UserRoles { get; set; } = default!;
    public virtual List<IdentityUserToken> UserTokens { get; set; } = default!;
    public virtual List<IdentityQr> Qrs { get; set; } = default!;
    public virtual List<IdentityLoginAttempt> LoginAttempts { get; set; } = default!;
    public virtual List<IdentitySession> Sessions { get; set; } = default!;

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
