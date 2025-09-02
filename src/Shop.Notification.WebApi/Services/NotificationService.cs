using Shop.Messages;

namespace Shop.Notification.WebApi.Services;

public class NotificationService : INotificationService
{
    public void Send(NotificacaoMessage message)
    {
        Console.WriteLine("Cliente Notificado");
    }
}
