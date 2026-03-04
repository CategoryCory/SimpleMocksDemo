using NSubstitute;
using OrderProcessor.Models;
using OrderProcessor.Services;

namespace OrderProcessor.Tests.UnitTests;

public sealed class PriceCalculatorTests
{
    private readonly ITaxService _taxService = Substitute.For<ITaxService>();

    [Fact]
    public async Task CalculateTotal_TaxableOrder_AppliesTax()
    {
        _taxService.IsTaxableAsync(Arg.Any<Order>()).Returns(true);

        var calculator = new PriceCalculator(_taxService);
        var order = new Order
        {
            Items =
            [
                new OrderItem { Name = "Widget", Quantity = 2, Price = 50.00m }
            ]
        };

        var total = await calculator.CalculateTotalAsync(order);

        Assert.Equal(110.00m, total);
    }

    [Fact]
    public async Task CalculateTotal_NonTaxableOrder_DoesNotApplyTax()
    {
        _taxService.IsTaxableAsync(Arg.Any<Order>()).Returns(false);

        var calculator = new PriceCalculator(_taxService);
        var order = new Order
        {
            Items =
            [
                new OrderItem { Name = "Widget", Quantity = 2, Price = 50.00m }
            ]
        };

        var total = await calculator.CalculateTotalAsync(order);

        Assert.Equal(100.00m, total);
    }

    [Fact(Skip = "Demonstrating test failure output")]
    public async Task CalculateTotal_TaxableOrder_DoesNotApplyTax_FailingDemo()
    {
        // INTENTIONAL FAILURE:
        // This test is deliberately wrong so we can demonstrate
        // how informative the test runner output is.

        _taxService.IsTaxableAsync(Arg.Any<Order>())
            .Returns(true);

        var calculator = new PriceCalculator(_taxService);

        var order = new Order
        {
            Items =
            [
                new OrderItem { Name = "Widget", Quantity = 2, Price = 50m }
            ]
        };

        var total = await calculator.CalculateTotalAsync(order);

        // Incorrect expectation on purpose:
        Assert.Equal(100m, total);
    }
}
