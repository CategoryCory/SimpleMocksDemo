using OrderProcessor.Models;
using OrderProcessor.Services;

namespace OrderProcessor.Tests.UnitTests;

public sealed class OrderValidatorTests
{
    private readonly OrderValidator _orderValidator = new();

    [Fact]
    public void Validate_ValidOrder_DoesNotThrow()
    {
        var order = new Order
        {
            Items =
            [
                new OrderItem { Name = "Widget", Quantity = 1, Price = 10.00m }
            ]
        };

        _orderValidator.Validate(order);
    }

    [Fact]
    public void Validate_OrderWithNoItems_ThrowsException()
    {
        var order = new Order();

        var ex = Assert.Throws<InvalidOperationException>(() => _orderValidator.Validate(order));

        Assert.Equal("Order must contain at least one item.", ex.Message);
    }
}
