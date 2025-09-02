using Shop.Messages;

namespace Shop.Notification.Service;

public record NotificacaoRequest
{
    public NotificationType Type { get; init; }
    public required string Destination { get; init; }
    public string? Body { get; init; }
}