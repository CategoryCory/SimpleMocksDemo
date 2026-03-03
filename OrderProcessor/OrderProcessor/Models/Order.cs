namespace OrderProcessor.Models;

/// <summary>
/// Represents a customer's order, containing multiple items.
/// </summary>
public sealed class Order
{
    /// <summary>
    /// A unique identifier for the order, generated automatically when the order is created.
    /// </summary>
    public Guid OrderId { get; init; } = Guid.NewGuid();

    /// <summary>
    /// The list of items included in the order.
    /// </summary>
    public IReadOnlyList<OrderItem> Items { get; init; } = [];
}
