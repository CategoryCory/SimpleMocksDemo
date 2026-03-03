using OrderProcessor.Models;

namespace OrderProcessor.Services;

/// <summary>
/// Defines the contract for a service that determines whether an order is taxable.
/// </summary>
public interface ITaxService
{
    /// <summary>
    /// Determines whether the given order is subject to tax based on its contents.
    /// </summary>
    /// <param name="order">The order to evaluate for taxability.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation and returns a boolean indicating whether
    /// the order is taxable.
    /// </returns>
    Task<bool> IsTaxableAsync(Order order, CancellationToken cancellationToken = default);
}
