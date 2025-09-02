using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shop.Messages;

namespace Shop.Order.Service.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;

    public OrdersController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitOrder([FromBody] SubmitOrderRequest request)
    {
        var orderId = Guid.NewGuid();

        await _publishEndpoint.Publish(new OrderMessage
        {
            OrderId = orderId,
            Total = request.Total,
            Email = request.Email,
            OrderDate = request.OrderDate,
            Items = request.Items.Select(x => new OrderItemMessage
            {
                Code = x.Code,
                Description = x.Description,
                Quantity = x.Quantity
            })
        });

        return Ok(new { OrderId = orderId });
    }
}

public record SubmitOrderRequest
{
    public Guid OrderId { get; init; }
    public decimal Total { get; init; }
    public string Email { get; init; }
    public DateTime OrderDate { get; init; }
    public IEnumerable<SubmitOrderItemRequest> Items { get; init; } = [];
}

public record SubmitOrderItemRequest
{
    public int Code { get; init; }
    public string Description { get; init; }
    public int Quantity { get; init; }
}