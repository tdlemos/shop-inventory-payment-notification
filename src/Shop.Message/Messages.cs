namespace Shop.Messages;

public record OrderMessage
{
    public Guid OrderId { get; init; }
    public decimal Total { get; init; }
    public required string Email { get; init; }
    public DateTime OrderDate { get; init; }
    public IEnumerable<OrderItemMessage> Items { get; init; } = [];
}

public record OrderItemMessage {
    public int Code { get; init; }
    public required string Description { get; init; }
    public int Quantity { get; init; }
}

public record PaymentProcessMessage
{
    public Guid OrderId { get; init; }
    public decimal Amount { get; init; }
}

public record PaymentProcessedMessage
{
    public Guid OrderId { get; init; }
    public string PaymentIntentId { get; init; }
}

public record ReserveInventoryMessage
{
    public Guid OrderId { get; init; }
}

public record InventoryReservedMessage
{
    public Guid OrderId { get; init; }
}

public record RefundPayment
{
    public Guid OrderId { get; init; }
    public decimal Amount { get; init; }
}

public record OrderConfirmed
{
    public Guid OrderId { get; init; }
}

public record OrderFailed
{
    public Guid OrderId { get; init; }
    public string? Reason { get; init; }
}

public record BuyerNotification
{
    public Guid OrderId { get; init; }
}

public record NotificacaoMessage
{
    public Guid OrderId { get; init; }
    public NotificationType Type { get; init; }
    public string Destination { get; init; }
    public string Body { get; init; }
}

public enum NotificationType
{
    Email,
    Sms
}

public record NotifieddMessage
{
    public Guid OrderId { get; init; }
}