using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations.Schema;

namespace CusstomAuth.EntityFrameworkCore.Audits;

public class AuditItem
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string By { get; set; } = default!;
    public string Event { get; set; } = default!;
    public string ItemId { get; set; } = default!;
    public string ItemType { get; set; } = default!;
    public string TransactionId { get; set; } = default!;
    public List<PropertyInfo> Changes { get; set; } = default!;

    [NotMapped]
    public List<PropertyEntry> TempProperties { get; set; } = default!;
}
