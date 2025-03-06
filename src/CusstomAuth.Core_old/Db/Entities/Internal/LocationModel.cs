namespace CusstomAuth.Core.Db.Entities.Internal;

public class LocationModel
{
    public string Ip { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public string Provider { get; set; }
}
