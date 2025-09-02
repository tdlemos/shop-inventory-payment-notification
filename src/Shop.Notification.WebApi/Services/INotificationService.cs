using Shop.Messages;

namespace Shop.Notification.WebApi.Services;
public interface INotificationService
{
    void Send(NotificacaoMessage message);
}