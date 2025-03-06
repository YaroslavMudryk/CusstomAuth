using CusstomAuth.Core.Services.Notifications.Dtos;

namespace CusstomAuth.Core.Services.Notifications;

public interface INotificationService
{
    Task SendNotificationAsync(NotificationDto notification);
}
