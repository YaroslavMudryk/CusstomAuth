using Microsoft.EntityFrameworkCore;

namespace CusstomAuth.Core.Db.Audits;

public class AuditRepo(TimeProvider timeProvider, ICurrentContext currentContext)
{
    public List<AuditItem> GetAuditsFromEntries(AuthDbContext context)
    {
        var utcNow = timeProvider.GetUtcNow().UtcDateTime;
        return context.ChangeTracker.Entries()
            .Where(s => s.Entity is IVersioning &&
                    (s.State == EntityState.Added ||
                    s.State == EntityState.Modified ||
                    s.State == EntityState.Deleted))
            .Select(entry =>
            {
                return AuditBuilder.NewDefaultAudit()
                    .SoftDeleted(entry.Entity is ISoftDeletable sd && sd.DeletedAt != null)
                    .On(entry.State)
                    .With(s =>
                    {
                        s.ItemId = entry.Properties.Single(s => s.Metadata.IsPrimaryKey()).CurrentValue.ToString();
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

    public Task SaveAuditsAsync(AuthDbContext context, List<AuditItem> audits)
    {
        audits.ForEach(audit =>
        {
            audit.TempProperties.ForEach(tempProperty =>
            {
                if (tempProperty.Metadata.IsPrimaryKey())
                {
                    audit.ItemId = tempProperty.CurrentValue.ToString();
                    var idProperty = audit.Changes.FirstOrDefault(s => s.Name == tempProperty.Metadata.Name);
                    idProperty.NewValue = audit.ItemId;
                }
                else
                {
                    var currentProperty = audit.Changes.FirstOrDefault(s => s.Name == tempProperty.Metadata.Name);
                    currentProperty.NewValue = tempProperty.CurrentValue.ToString();
                }
            });
        });

        context.Audits.AddRange(audits);
        return context.SaveChangesAsync(true);
    }
}
