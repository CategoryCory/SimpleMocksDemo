using OrderProcessor.Models;
using OrderProcessor.Services;

namespace OrderProcessor;

/// <summary>
/// The main entry point for the OrderProcessor application.
/// </summary>
class Program
{
    /// <summary>
    /// The main method initializes the necessary services, creates a sample order, validates it,
    /// and calculates the total price, including tax if applicable, before printing the result
    /// to the console.
    /// </summary>
    /// <param name="args">The command-line arguments passed to the application.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
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
