using OrderProcessor.Models;
using OrderProcessor.Services;

namespace OrderProcessor;

class Program
{
    static async Task Main(string[] args)
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:8000")
        };

        var taxService = new HttpTaxService(httpClient);
        var priceCalculator = new PriceCalculator(taxService);
        var orderValidator = new OrderValidator();

        var order = new Order
        {
            Items =
            [
                new OrderItem { Name = "Widget", Quantity = 2, Price = 70.00m }
            ]
        };

        orderValidator.Validate(order);
    
        var total = await priceCalculator.CalculateTotalAsync(order);
        Console.WriteLine($"Order total: {total:C}");
    }
}
