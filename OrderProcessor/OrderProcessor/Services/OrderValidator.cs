using OrderProcessor.Models;

namespace OrderProcessor.Services;

public sealed class OrderValidator
{
    public void Validate(Order order)
    {
        ArgumentNullException.ThrowIfNull(order, nameof(order));

        if (order.Items is null || !order.Items.Any())
            throw new InvalidOperationException("Order must contain at least one item.");
        
        foreach (var item in order.Items)
        {
            if (item.Quantity <= 0)
                throw new InvalidOperationException($"Item '{item.Name}' must have a quantity greater than zero.");
            
            if (item.Price < 0)
                throw new InvalidOperationException($"Item '{item.Name}' cannot have a negative price.");
        }
    }
}
