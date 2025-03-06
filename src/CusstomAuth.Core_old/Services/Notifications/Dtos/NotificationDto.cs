namespace CusstomAuth.Core.Services.Notifications.Dtos;

public class NotificationDto
{
    public string Title { get; set; }
    public string Body { get; set; }
    public Dictionary<string, List<string>> Data { get; set; }
}
