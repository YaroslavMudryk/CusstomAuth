namespace CusstomAuth.Core.Db.Entities.Internal;

public class AppModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Version { get; set; }
    public string ImageUrl { get; set; }

    public static AppModel New(AuthApp app, string appVersion)
    {
        return new AppModel
        {
            Id = app.Id,
            ImageUrl = app.Image,
            Name = app.Name,
            Version = appVersion
        };
    }
}
