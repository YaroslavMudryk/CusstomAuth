namespace CusstomAuth.Core.Db.Audits;

public interface ISoftDeletable
{
    public DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; }
}
