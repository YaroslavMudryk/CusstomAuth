using CusstomAuth.Core.Db.Audits;

namespace CusstomAuth.Core.Db.Entities;

public class AuthApp : AuditableSoftDeletedBaseModelWithIdentity<int>
{
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public bool IsActive { get; set; }
    public DateTime ActiveFrom { set; get; }
    public DateTime? ActiveTo { set; get; }

    public bool IsActiveByTime(DateTime dateTime)
    {
        return dateTime >= ActiveFrom && dateTime <= ActiveTo;
    }
}
