namespace CusstomAuth;

public class IdentityBaseModel<TKey> : BaseModel
{
    public virtual TKey Id { get; set; } = default!;
}

public class IdentityBaseModel : BaseModel
{
    public int Id { get; set; }
}

public class BaseModel : IAudit, ISoftDelete
{
    public virtual DateTime CreatedAt { get; set; }
    public virtual string CreatedBy { get; set; } = default!;
    public virtual DateTime UpdatedAt { get; set; }
    public virtual string UpdatedBy { get; set; } = default!;
    public virtual int Version { get; set; }

    public virtual DateTime? DeletedAt { get; set; }
    public virtual string DeletedBy { get; set; } = default!;
}
