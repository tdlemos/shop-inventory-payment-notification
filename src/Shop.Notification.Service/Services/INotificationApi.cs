using Refit;

namespace Shop.Notification.Service.Services;

public interface INotificationApi
{
    [Post("/notification/notify")]
    Task<bool> SendNotification([Body] NotificacaoRequest request);
}
