namespace CusstomAuth;

public class AppModel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Version { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;

    public static AppModel New(IdentityApp app, string appVersion)
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
