using OrderProcessor.Models;

namespace OrderProcessor.Services;

/// <summary>
/// A service responsible for validating orders before they are processed.
/// </summary>
public sealed class OrderValidator
{
    /// <summary>
    /// Validates the given order by checking that it contains at least one item and that each
    /// item has a positive quantity and non-negative price.
    /// </summary>
    /// <param name="order">The order to validate.</param>
    /// <exception cref="InvalidOperationException">Thrown when the order is invalid.</exception>
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
