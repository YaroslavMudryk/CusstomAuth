namespace CusstomAuth;

public class LocationModel
{
    public string Ip { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Region { get; set; } = default!;
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string Provider { get; set; } = default!;
}
