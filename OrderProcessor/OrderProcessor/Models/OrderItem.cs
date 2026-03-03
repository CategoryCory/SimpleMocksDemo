namespace OrderProcessor.Models;

/// <summary>
/// Represents an individual item in a customer's order.
/// </summary>
public sealed class OrderItem
{
    /// <summary>
    /// The name of the item being ordered.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// The quantity of the item being ordered.
    /// </summary>
    public int Quantity { get; init; }

    /// <summary>
    /// The price of a single unit of the item being ordered.
    /// </summary>
    public decimal Price { get; init; }
}
