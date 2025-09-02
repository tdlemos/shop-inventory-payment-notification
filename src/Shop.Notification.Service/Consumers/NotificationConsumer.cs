using MassTransit;
using Shop.Messages;
using Shop.Notification.Service.Services;

namespace Shop.Notification.Service.Consumers;

public class NotificationConsumer : IConsumer<NotificacaoMessage>
{
    private readonly ILogger<NotificationConsumer> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly INotificationApi _notificationApi;

    public NotificationConsumer(ILogger<NotificationConsumer> logger, IPublishEndpoint publishEndpoint,
        INotificationApi notificationApi)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _notificationApi = notificationApi;
    }

    public async Task Consume(ConsumeContext<NotificacaoMessage> context)
    {
        _logger.LogInformation("Processing payment for order: {OrderId}", context.Message.OrderId);

        await Task.Delay(1000);

        var response = await _notificationApi.SendNotification(new()
        {
            Destination = context.Message.Destination,
            Body = context.Message.Body,
            Type = context.Message.Type,
        });

    }
}
