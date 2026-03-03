namespace OrderProcessor.Models;

public sealed class OrderItem
{
    public string Name { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal Price { get; init; }
}
