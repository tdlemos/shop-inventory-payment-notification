using MassTransit;
using Shop.Messages;

namespace Shop.Order.Service.Sagas;

public class OrderSagaMachine : MassTransitStateMachine<OrderState>
{
    public State ReservingInventory { get; private set; }
    public State ProcessingPayment { get; private set; }
    public State NotifyingBuyer { get; private set; }
    public State Completed { get; private set; }
    public State Failed { get; private set; }


    public Event<OrderMessage> OrderSubmitted { get; private set; }
    public Event<InventoryReservedMessage> InventoryReserved { get; private set; }
    public Event<PaymentProcessedMessage> PaymentProcessed { get; private set; }
    public Event<NotificacaoMessage> NotifyBuyer { get; private set; }
    public Event<OrderFailed> OrderFailed { get; private set; }

    public OrderSagaMachine()
    {
        Event(() => OrderSubmitted, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => InventoryReserved, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => PaymentProcessed, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => NotifyBuyer, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => OrderFailed, x => x.CorrelateById(m => m.Message.OrderId));
        
        InstanceState(x => x.CurrentState);

        Initially(
            When(OrderSubmitted)
                .Then(context =>
                {
                    context.Saga.OrderTotal = context.Message.Total;
                    context.Saga.CustomerEmail = context.Message.Email;
                    context.Saga.OrderDate = DateTime.UtcNow;
                })
                .PublishAsync(context => context.Init<ReserveInventoryMessage>(new
                {
                    OrderId = context.Saga.CorrelationId,
                    Amount = context.Saga.OrderTotal
                }))
                .TransitionTo(ReservingInventory)
        );

        During(ReservingInventory,
            When(InventoryReserved)
                .PublishAsync(context => context.Init<PaymentProcessMessage>(new
                {
                    OrderId = context.Saga.CorrelationId
                }))
                .TransitionTo(ProcessingPayment),
            When(OrderFailed)
                .PublishAsync(context => context.Init<RefundPayment>(new
                {
                    OrderId = context.Saga.CorrelationId,
                    Amount = context.Saga.OrderTotal
                }))
                .TransitionTo(Failed)
                .Finalize()
        );

        During(ProcessingPayment,
            When(PaymentProcessed)
                .PublishAsync(context => context.Init<NotificacaoMessage>(new
                {
                    OrderId = context.Saga.CorrelationId,
                    Type = 0,
                    Destination = context.Saga.CustomerEmail,
                    Body = $"Sua compra foi confirmada N.{context.Saga.CorrelationId.ToString()}"
                }))
                .TransitionTo(NotifyingBuyer),
            When(OrderFailed)
                .TransitionTo(Failed)
                .Finalize()
        );

        During(NotifyingBuyer,
            When(InventoryReserved)
                .PublishAsync(context => context.Init<OrderConfirmed>(new
                {
                    OrderId = context.Saga.CorrelationId
                }))
                .TransitionTo(Completed)
                .Finalize(),
            When(OrderFailed)
                .PublishAsync(context => context.Init<RefundPayment>(new
                {
                    OrderId = context.Saga.CorrelationId,
                    Amount = context.Saga.OrderTotal
                }))
                .TransitionTo(Failed)
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }


}
