namespace CusstomAuth;

public interface ISoftDelete
{
    public DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; }
}
