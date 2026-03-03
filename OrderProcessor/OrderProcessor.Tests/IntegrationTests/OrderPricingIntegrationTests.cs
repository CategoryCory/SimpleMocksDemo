using OrderProcessor.Models;
using OrderProcessor.Services;

namespace OrderProcessor.Tests.IntegrationTests;

public sealed class OrderPricingIntegrationTests
{
    [Fact]
    public async Task OrderOver100_IsTaxed()
    {
        var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:8000") };

        var taxService = new HttpTaxService(httpClient);
        var calculator = new PriceCalculator(taxService);
        var validator = new OrderValidator();

        var order = new Order
        {
            Items =
            [
                new OrderItem { Name = "Expensive Widget", Quantity = 3, Price = 70.00m }
            ]
        };

        validator.Validate(order);

        var total = await calculator.CalculateTotalAsync(order);

        Assert.Equal(231.00m, total);
    }

    [Fact]
    public async Task OrderUnder100_IsNotTaxed()
    {
        var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:8000") };

        var taxService = new HttpTaxService(httpClient);
        var calculator = new PriceCalculator(taxService);
        var validator = new OrderValidator();

        var order = new Order
        {
            Items =
            [
                new OrderItem { Name = "Expensive Widget", Quantity = 1, Price = 70.00m }
            ]
        };

        validator.Validate(order);

        var total = await calculator.CalculateTotalAsync(order);

        Assert.Equal(70.00m, total);
    }
}
