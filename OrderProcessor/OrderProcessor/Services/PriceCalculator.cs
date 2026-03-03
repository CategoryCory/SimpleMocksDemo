using OrderProcessor.Models;

namespace OrderProcessor.Services;

/// <summary>
/// A service responsible for calculating the total price of an order, including tax if applicable.
/// </summary>
public sealed class PriceCalculator
{
    /// <summary>
    /// The ITaxService used to determine whether an order is taxable.
    /// </summary>
    private readonly ITaxService _taxService;

    /// <summary>
    /// Initializes a new instance of the PriceCalculator class.
    /// </summary>
    /// <param name="taxService">The ITaxService to use for determining taxability.</param>
    /// <exception cref="ArgumentNullException">Thrown when taxService is null.</exception>
    public PriceCalculator(ITaxService taxService)
    {
        _taxService = taxService ?? throw new ArgumentNullException(nameof(taxService));
    }

    /// <summary>
    /// Calculates the total price of the given order, including tax if applicable, by summing the price of each
    /// item and applying a 10% tax if the order is taxable.
    /// </summary>
    /// <param name="order">The order for which to calculate the total price.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation and returns the total price of the order.
    /// </returns>
    public async Task<decimal> CalculateTotalAsync(Order order, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);

        var subtotal = order.Items.Sum(item => item.Price * item.Quantity);

        var isTaxable = await _taxService.IsTaxableAsync(order, cancellationToken);

        return isTaxable ? subtotal * 1.1m : subtotal;
    }
}
