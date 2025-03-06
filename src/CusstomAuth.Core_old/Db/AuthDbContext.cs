using CusstomAuth.Core.Db.Audits;
using CusstomAuth.Core.Db.Entities;
using CusstomAuth.Core.Db.Entities.Internal;
using CusstomAuth.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace CusstomAuth.Core.Db;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{
    public DbSet<AuthApp> Apps { get; set; }
    public DbSet<AuthAppClaim> AppClaims { get; set; }
    public DbSet<AuthBlock> Blocks { get; set; }
    public DbSet<AuthClaim> Claims { get; set; }
    public DbSet<AuthConfirm> Confirms { get; set; }
    public DbSet<AuthContact> Contacts { get; set; }
    public DbSet<AuthDevice> Devices { get; set; }
    public DbSet<AuthExternalLogin> ExternalLogins { get; set; }
    public DbSet<AuthLoginAttempt> LoginAttempts { get; set; }
    public DbSet<AuthMfa> Mfas { get; set; }
    public DbSet<AuthPassword> Passwords { get; set; }
    public DbSet<AuthQr> Qrs { get; set; }
    public DbSet<AuthRole> Roles { get; set; }
    public DbSet<AuthRoleClaim> RoleClaims { get; set; }
    public DbSet<AuthSession> Sessions { get; set; }
    public DbSet<AuthRefreshToken> RefreshTokens { get; set; }
    public DbSet<AuthUser> Users { get; set; }
    public DbSet<AuthUserRole> UserRoles { get; set; }

    public DbSet<AuditItem> Audits { get; set; }

    public Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolation = IsolationLevel.Unspecified) => this.Database.BeginTransactionAsync(isolation);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthApp>(entity =>
        {
            entity.ToTable("apps");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        modelBuilder.Entity<AuthAppClaim>(entity =>
        {
            entity.ToTable("appClaims");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        modelBuilder.Entity<AuthBlock>(entity =>
        {
            entity.ToTable("blocks");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        modelBuilder.Entity<AuthClaim>(entity =>
        {
            entity.ToTable("claims");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        modelBuilder.Entity<AuthConfirm>(entity =>
        {
            entity.ToTable("confirms");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        modelBuilder.Entity<AuthContact>(entity =>
        {
            entity.ToTable("contacts");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        modelBuilder.Entity<AuthDevice>(entity =>
        {
            entity.ToTable("devices");
            entity.HasKey(s => s.Id);
            entity.HasIndex(s => s.DeviceIdentifier).HasDatabaseName("idxDeviceIdentifier");
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        modelBuilder.Entity<AuthExternalLogin>(entity =>
        {
            entity.ToTable("externalLogins");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        modelBuilder.Entity<AuthLoginAttempt>(entity =>
        {
            entity.ToTable("loginAttempts");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Client).HasConversion(
                c => c.ToJson(),
                c => c.FromJson<ClientModel>());
            entity.Property(s => s.Location).HasConversion(
                c => c.ToJson(),
                c => c.FromJson<LocationModel>());
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        modelBuilder.Entity<AuthMfa>(entity =>
        {
            entity.ToTable("mfas");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        modelBuilder.Entity<AuthPassword>(entity =>
        {
            entity.ToTable("passwords");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        modelBuilder.Entity<AuthQr>(entity =>
        {
            entity.ToTable("qrs");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        modelBuilder.Entity<AuthRole>(entity =>
        {
            entity.ToTable("roles");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        modelBuilder.Entity<AuthRoleClaim>(entity =>
        {
            entity.ToTable("roleClaims");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        modelBuilder.Entity<AuthSession>(entity =>
        {
            entity.ToTable("sessions");
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

        modelBuilder.Entity<AuthRefreshToken>(entity =>
        {
            entity.ToTable("refreshTokens");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });

        modelBuilder.Entity<AuthUser>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(s => s.DeletedAt == null);
        });

        modelBuilder.Entity<AuthUserRole>(entity =>
        {
            entity.ToTable("userRoles");
            entity.HasKey(s => s.Id);
            entity.HasQueryFilter(qf => qf.DeletedAt == null);
        });


        modelBuilder.Entity<AuditItem>(entity =>
        {
            entity.ToTable("audits");
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
        var auditRepo = this.GetService<AuditRepo>();
        return auditRepo.GetAuditsFromEntries(this);
    }

    protected virtual Task OnAfterSaveChangesAsync(List<AuditItem> audits)
    {
        if (audits == null || audits.Count == 0)
            return Task.CompletedTask;

        return this.GetService<AuditRepo>().SaveAuditsAsync(this, audits);
    }

    protected virtual void AuditItemsBeforeSaveChanges()
    {
        var timeProvider = this.GetService<TimeProvider>();
        var userContext = this.GetService<ICurrentContext>();
        var by = userContext.User.Id.ToString();
        var utcNow = timeProvider.GetUtcNow().UtcDateTime;
        var entries = ChangeTracker.Entries().Where(s => s.Entity is IAuditable).Select(entry =>
        {
            var entity = entry.Entity as IAuditable;
            ArgumentNullException.ThrowIfNull(entity);
            if (entry.State == EntityState.Deleted && entry.Entity is ISoftDeletable sd)
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
