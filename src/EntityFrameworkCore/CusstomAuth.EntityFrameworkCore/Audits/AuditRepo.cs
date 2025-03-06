using Microsoft.EntityFrameworkCore;

namespace CusstomAuth.EntityFrameworkCore.Audits;

public class AuditRepo<
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
    TUserToken>(TimeProvider timeProvider, ICurrentContext currentContext)
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
    public List<AuditItem> GetAuditsFromEntries(IdentityDbContext<TUser, TRole, TKey, TApp, TAppClaim, TBlock, TClaim, TConfirm, TContact, TDevice, TLoginAttempt, TMfa, TPassword, TQr, TRefreshToken, TRoleClaim, TSession, TUserRole, TUserToken> context)
    {
        var utcNow = timeProvider.GetUtcNow().UtcDateTime;
        return context.ChangeTracker.Entries()
            .Where(s => s.Entity is IVersion &&
                    (s.State == EntityState.Added ||
                    s.State == EntityState.Modified ||
                    s.State == EntityState.Deleted))
            .Select(entry =>
            {
                return AuditBuilder.NewDefaultAudit()
                    .SoftDeleted(entry.Entity is ISoftDelete sd && sd.DeletedAt != null)
                    .On(entry.State)
                    .With(s =>
                    {
                        s.ItemId = entry.Properties.Single(s => s.Metadata.IsPrimaryKey()).CurrentValue!.ToString()!;
                        s.ItemType = entry.Entity.GetType().Name;
                        s.TransactionId = context.ContextId.InstanceId.ToString();
                        s.CreatedAt = utcNow;
                        s.TempProperties = entry.Properties.Where(s => s.IsTemporary).ToList();
                        s.By = currentContext.User.Id.ToString();
                    })
                    .WithChanges(entry)
                    .Build();
            }).ToList();
    }

    public Task SaveAuditsAsync(IdentityDbContext<TUser, TRole, TKey, TApp, TAppClaim, TBlock, TClaim, TConfirm, TContact, TDevice, TLoginAttempt, TMfa, TPassword, TQr, TRefreshToken, TRoleClaim, TSession, TUserRole, TUserToken> context, List<AuditItem> audits)
    {
        audits.ForEach(audit =>
        {
            audit.TempProperties.ForEach(tempProperty =>
            {
                if (tempProperty.Metadata.IsPrimaryKey())
                {
                    audit.ItemId = tempProperty.CurrentValue!.ToString()!;
                    var idProperty = audit.Changes.FirstOrDefault(s => s.Name == tempProperty.Metadata.Name);
                    idProperty!.NewValue = audit.ItemId!;
                }
                else
                {
                    var currentProperty = audit.Changes.FirstOrDefault(s => s.Name == tempProperty.Metadata.Name);
                    currentProperty!.NewValue = tempProperty.CurrentValue!.ToString()!;
                }
            });
        });

        context.Audits.AddRange(audits);
        return context.SaveChangesAsync(true);
    }
}
