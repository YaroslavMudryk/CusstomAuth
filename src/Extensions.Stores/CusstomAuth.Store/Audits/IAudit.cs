namespace CusstomAuth;

public interface IAudit : IVersion
{
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
}
