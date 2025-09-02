using Microsoft.AspNetCore.Mvc;
using Shop.Messages;
using Shop.Notification.WebApi.Services;

namespace Shop.Notification.WebApi.Controllers;

[ApiController]
[Route("notification")]
public class NotificationController(ILogger<NotificationController> logger, INotificationService notificationService) : ControllerBase
{
    private readonly ILogger<NotificationController> _logger = logger;
    private readonly INotificationService _notificationService = notificationService;

    [HttpPost("notify")]
    public bool Notificar([FromBody] NotificacaoRequest request)
    {
        _notificationService.Send(new NotificacaoMessage
        {
            Type = NotificationType.Email,
            Body = request.Body,
            Destination = request.Destination
        });

        return true;
    }
}

public record NotificacaoRequest
{
    public required string Type { get; init; }
    public string Destination { get; init; }
    public string Body { get; init; }
}