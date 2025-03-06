using CusstomAuth.Core.Services.Notifications.Dtos;

namespace CusstomAuth.Core.Services.Notifications;

public class FakeNotificationService : INotificationService
{
    public async Task SendNotificationAsync(NotificationDto notification)
    {
        await Task.CompletedTask;
    }
}
