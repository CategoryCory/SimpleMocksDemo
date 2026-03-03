namespace OrderProcessor.Models;

public sealed class Order
{
    public Guid OrderId { get; init; } = Guid.NewGuid();
    public IReadOnlyList<OrderItem> Items { get; init; } = [];
}
