using System.ComponentModel.DataAnnotations.Schema;

namespace CusstomAuth.Core.Db.Audits;

public class BaseModel : IAuditable
{
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public int Version { get; set; }
}

public class BaseModelWithIdentity<TId> : BaseModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TId Id { get; set; }
}

public class AuditableBaseModelWithIdentity<TId> : BaseModelWithIdentity<TId>, IAuditable
{

}

public class AuditableSoftDeletedBaseModelWithIdentity<TId> : AuditableBaseModelWithIdentity<TId>, ISoftDeletable
{
    public DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; }
}

public class SoftDeletedBaseModelWithIdentity<TId> : BaseModelWithIdentity<TId>, ISoftDeletable
{
    public DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; }
}

public class BaseModelWithoutIdentity<TId> : BaseModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public TId Id { get; set; }
}

public class AuditableBaseModelWithoutIdentity<TId> : BaseModelWithoutIdentity<TId>, IAuditable
{

}

public class AuditableSoftDeletedBaseModelWithoutIdentity<TId> : AuditableBaseModelWithoutIdentity<TId>, ISoftDeletable
{
    public DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; }
}

public class SoftDeletedBaseModelWithoutIdentity<TId> : BaseModelWithoutIdentity<TId>, ISoftDeletable
{
    public DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; }
}
