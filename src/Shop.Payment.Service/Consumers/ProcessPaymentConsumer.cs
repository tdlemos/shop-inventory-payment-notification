using MassTransit;
using Shop.Messages;

namespace Shop.Payment.Service.Consumers;

public class ProcessPaymentConsumer : IConsumer<PaymentProcessMessage>
{
    private readonly ILogger<ProcessPaymentConsumer> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public ProcessPaymentConsumer(ILogger<ProcessPaymentConsumer> logger, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<PaymentProcessMessage> context)
    {
        _logger.LogInformation("Processing payment for order: {OrderId}", context.Message.OrderId);

        // Simulate payment processing
        await Task.Delay(1000);

        // Inserir lógica de pagamento

        // Lógica aleatório para simular sucesso de pagamento ou falha
        if (Random.Shared.Next(100) < 90)
        {
            await _publishEndpoint.Publish(new PaymentProcessedMessage
            {
                OrderId = context.Message.OrderId,
                PaymentIntentId = $"pi_{Guid.NewGuid():N}"
            });
        }
        else
        {
            await _publishEndpoint.Publish(new OrderFailed
            {
                OrderId = context.Message.OrderId,
                Reason = "Payment failed"
            });
        }
    }
}
