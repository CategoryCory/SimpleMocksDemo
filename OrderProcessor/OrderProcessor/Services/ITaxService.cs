using OrderProcessor.Models;

namespace OrderProcessor.Services;

public interface ITaxService
{
    Task<bool> IsTaxableAsync(Order order, CancellationToken cancellationToken = default);
}
