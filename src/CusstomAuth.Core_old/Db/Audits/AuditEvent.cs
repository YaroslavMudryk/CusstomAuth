namespace CusstomAuth.Core.Db.Audits;

public static class AuditEvent
{
    public static string Create = nameof(Create);
    public static string Update = nameof(Update);
    public static string SoftDelete = nameof(SoftDelete);
    public static string Delete = nameof(Delete);
}
