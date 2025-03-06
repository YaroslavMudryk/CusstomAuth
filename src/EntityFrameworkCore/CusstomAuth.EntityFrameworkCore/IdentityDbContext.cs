using CusstomAuth.Core.Extensions;
using CusstomAuth.EntityFrameworkCore.Audits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CusstomAuth.EntityFrameworkCore;

public class IdentityDbContext : IdentityDbContext<IdentityUser, IdentityRole, int>
{
    public IdentityDbContext(DbContextOptions options) : base(options) { }
    protected IdentityDbContext() { }
}

public class IdentityDbContext<TUser> : IdentityDbContext<TUser, IdentityRole, int, IdentityApp, IdentityAppClaim, IdentityBlock, IdentityClaim, IdentityConfirm, IdentityContact, IdentityDevice, IdentityLoginAttempt, IdentityMfa, IdentityPassword, IdentityQr, IdentityRefreshToken, IdentityRoleClaim, IdentitySession, IdentityUserRole, IdentityUserToken>
    where TUser : IdentityUser
{
    public IdentityDbContext(DbContextOptions options) : base(options) { }
    protected IdentityDbContext() { }
}

public class IdentityDbContext<TUser, TRole, TKey> : IdentityDbContext<TUser, TRole, TKey, IdentityApp, IdentityAppClaim, IdentityBlock, IdentityClaim, IdentityConfirm, IdentityContact, IdentityDevice, IdentityLoginAttempt, IdentityMfa, IdentityPassword, IdentityQr, IdentityRefreshToken, IdentityRoleClaim, IdentitySession, IdentityUserRole, IdentityUserToken>
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
{
    public IdentityDbContext(DbContextOptions options) : base(options) { }
    protected IdentityDbContext() { }
}

public abstract class IdentityDbContext<
    TUser,
    TRole,
    TKey,
    TApp,
    TAppClaim,
    TBlock,
    TClaim,
    TConfirm,
    TContact,
    TDevice,
    TLoginAttempt,
    TMfa,
    TPassword,
    TQr,
    TRefreshToken,
    TRoleClaim,
    TSession,
    TUserRole,
    TUserToken> : DbContext
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
    where TApp : IdentityApp
    where TAppClaim : IdentityAppClaim
    where TBlock : IdentityBlock
    where TClaim : IdentityClaim
    where TConfirm : IdentityConfirm
    where TContact : IdentityContact
    where TDevice : IdentityDevice
    where TLoginAttempt : IdentityLoginAttempt
    where TMfa : IdentityMfa
    where TPassword : IdentityPassword
    where TQr : IdentityQr
    where TRefreshToken : IdentityRefreshToken
    where TRoleClaim : IdentityRoleClaim
    where TSession : IdentitySession
    where TUserRole : IdentityUserRole
    where TUserToken : IdentityUserToken
{
    public IdentityDbContext(DbContextOptions options) : base(options) { }
    protected IdentityDbContext() { }

    public virtual DbSet<TUser> Users { get; set; } = default!;
    public virtual DbSet<TRole> Roles { get; set; } = default!;
    public virtual DbSet<TApp> Apps { get; set; } = default!;
    public virtual DbSet<TAppClaim> AppClaims { get; set; } = default!;
    public virtual DbSet<TBlock> Blocks { get; set; } = default!;
    public virtual DbSet<TClaim> Claims { get; set; } = default!;
    public virtual DbSet<TConfirm> Confirms { get; set; } = default!;
    public virtual DbSet<TContact> Contacts { get; set; } = default!;
    public virtual DbSet<TDevice> Devices { get; set; } = default!;
    public virtual DbSet<TLoginAttempt> LoginAttempts { get; set; } = default!;
    public virtual DbSet<TMfa> Mfas { get; set; } = default!;
    public virtual DbSet<TPassword> Passwords { get; set; } = default!;
    public virtual DbSet<TQr> Qrs { get; set; } = default!;
    public virtual DbSet<TRefreshToken> RefreshTokens { get; set; } = default!;
    public virtual DbSet<TRoleClaim> RoleClaims { get; set; } = default!;
    public virtual DbSet<TSession> Sessions { get; set; } = default!;
    public virtual DbSet<TUserRole> UserRoles { get; set; } = default!;
    public virtual DbSet<TUserToken> UserTokens { get; set; } = default!;
    public DbSet<AuditItem> Audits { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<TUser>(entity =>
        {
            entity.ToTable("caUsers");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(s => s.DeletedAt == null);
        });

        builder.Entity<TRole>(entity =>
        {
            entity.ToTable("caRoles");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        builder.Entity<TApp>(entity =>
        {
            entity.ToTable("caApps");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        builder.Entity<TAppClaim>(entity =>
        {
            entity.ToTable("caAppClaims");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        builder.Entity<TBlock>(entity =>
        {
            entity.ToTable("caBlocks");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        builder.Entity<TClaim>(entity =>
        {
            entity.ToTable("caClaims");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        builder.Entity<TConfirm>(entity =>
        {
            entity.ToTable("caConfirms");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        builder.Entity<TContact>(entity =>
        {
            entity.ToTable("caContacts");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        builder.Entity<TDevice>(entity =>
        {
            entity.ToTable("caDevices");
            entity.HasKey(s => s.Id);
            entity.HasIndex(s => s.DeviceIdentifier).HasDatabaseName("idxDeviceIdentifier");
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        builder.Entity<TLoginAttempt>(entity =>
        {
            entity.ToTable("caLoginAttempts");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Client).HasConversion(
                c => c.ToJson(),
                c => c.FromJson<ClientModel>());
            entity.Property(s => s.Location).HasConversion(
                c => c.ToJson(),
                c => c.FromJson<LocationModel>());
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        builder.Entity<TMfa>(entity =>
        {
            entity.ToTable("caMfas");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        builder.Entity<TPassword>(entity =>
        {
            entity.ToTable("caPasswords");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        builder.Entity<TQr>(entity =>
        {
            entity.ToTable("caQrs");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        builder.Entity<TRefreshToken>(entity =>
        {
            entity.ToTable("caRefreshTokens");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        builder.Entity<TRoleClaim>(entity =>
        {
            entity.ToTable("caRoleClaims");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        builder.Entity<TSession>(entity =>
        {
            entity.ToTable("caSessions");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.App).HasConversion(
                c => c.ToJson(),
                c => c.FromJson<AppModel>());
            entity.Property(s => s.Client).HasConversion(
                c => c.ToJson(),
                c => c.FromJson<ClientModel>());
            entity.Property(s => s.Location).HasConversion(
                c => c.ToJson(),
                c => c.FromJson<LocationModel>());
            entity.HasIndex(s => s.PublicId).HasDatabaseName("idxSessionPublicId");
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        builder.Entity<TUserRole>(entity =>
        {
            entity.ToTable("caUserRoles");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        builder.Entity<TUserToken>(entity =>
        {
            entity.ToTable("caUserLogins");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        builder.Entity<AuditItem>(entity =>
        {
            entity.ToTable("caAudits");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Changes).HasConversion(
                c => c.ToJson(),
                c => c.FromJson<List<PropertyInfo>>());
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AuditItemsBeforeSaveChanges();
        var audits = OnBeforeSaveChanges();

        var result = await base.SaveChangesAsync(cancellationToken);

        await OnAfterSaveChangesAsync(audits);
        return result;
    }

    protected virtual List<AuditItem> OnBeforeSaveChanges()
    {
        var auditRepo = this.GetService<AuditRepo<TUser, TRole, TKey, TApp, TAppClaim, TBlock, TClaim, TConfirm, TContact, TDevice, TLoginAttempt, TMfa, TPassword, TQr, TRefreshToken, TRoleClaim, TSession, TUserRole, TUserToken>>();
        return auditRepo.GetAuditsFromEntries(this);
    }

    protected virtual Task OnAfterSaveChangesAsync(List<AuditItem> audits)
    {
        if (audits == null || audits.Count == 0)
            return Task.CompletedTask;

        return this.GetService<AuditRepo<TUser, TRole, TKey, TApp, TAppClaim, TBlock, TClaim, TConfirm, TContact, TDevice, TLoginAttempt, TMfa, TPassword, TQr, TRefreshToken, TRoleClaim, TSession, TUserRole, TUserToken>>().SaveAuditsAsync(this, audits);
    }

    protected virtual void AuditItemsBeforeSaveChanges()
    {
        var timeProvider = this.GetService<TimeProvider>();
        var userContext = this.GetService<ICurrentContext>();
        var by = userContext.User.Id.ToString();
        var utcNow = timeProvider.GetUtcNow().UtcDateTime;
        var entries = ChangeTracker.Entries().Where(s => s.Entity is IAudit).Select(entry =>
        {
            var entity = entry.Entity as IAudit;
            ArgumentNullException.ThrowIfNull(entity);
            if (entry.State == EntityState.Deleted && entry.Entity is ISoftDelete sd)
            {
                sd.DeletedAt = utcNow;
                sd.DeletedBy = by;
                entry.State = EntityState.Modified;
            }
            switch (entry.State)
            {
                case EntityState.Modified:
                    entity.UpdatedAt = utcNow;
                    entity.UpdatedBy = by;
                    break;
                case EntityState.Added:
                    entity.CreatedAt = utcNow;
                    entity.CreatedBy = by;
                    entity.UpdatedAt = utcNow;
                    entity.UpdatedBy = by;
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Deleted:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            entity.Version++;
            return entry;
        }).ToList();
    }
}