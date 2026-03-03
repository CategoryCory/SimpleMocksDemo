using OrderProcessor.Models;

namespace OrderProcessor.Services;

public sealed class PriceCalculator
{
    private readonly ITaxService _taxService;

    public PriceCalculator(ITaxService taxService)
    {
        _taxService = taxService ?? throw new ArgumentNullException(nameof(taxService));
    }

    public async Task<decimal> CalculateTotalAsync(Order order, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);

        var subtotal = order.Items.Sum(item => item.Price * item.Quantity);

        var isTaxable = await _taxService.IsTaxableAsync(order, cancellationToken);

        return isTaxable ? subtotal * 1.1m : subtotal;
    }
}
