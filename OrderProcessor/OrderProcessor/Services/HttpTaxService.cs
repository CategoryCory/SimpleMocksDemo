using System.Net.Http.Json;
using System.Text.Json.Serialization;
using OrderProcessor.Models;

namespace OrderProcessor.Services;

/// <summary>
/// An implementation of ITaxService that makes an HTTP request to an external service to determine
/// whether an order is taxable.
/// </summary>
public sealed class HttpTaxService : ITaxService
{
    /// <summary>
    /// The HttpClient used to make requests to the external tax service.
    /// </summary>
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the HttpTaxService class with the specified HttpClient.
    /// </summary>
    /// <param name="httpClient">The HttpClient to use for making requests to the external tax service.</param>
    public HttpTaxService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Determines whether the given order is subject to tax by making an HTTP request to an external service.
    /// </summary>
    /// <param name="order">The order to evaluate for taxability.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation and returns a boolean indicating whether
    /// the order is taxable.
    /// </returns>
    public async Task<bool> IsTaxableAsync(Order order, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "/calculate-tax",
            new
            {
                order_id = order.OrderId,
                items = order.Items.Select(i => new
                {
                    name = i.Name,
                    quantity = i.Quantity,
                    price = i.Price
                })
            },
            cancellationToken
        );

        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<TaxResponse>(cancellationToken: cancellationToken);

        return dto?.IsTaxable ?? false;
    }

    /// <summary>
    /// A private record type used to deserialize the response from the external tax service.
    /// </summary>
    /// <param name="IsTaxable">Whether the order is taxable.</param>
    private sealed record TaxResponse(
        [property: JsonPropertyName("is_taxable")]
        bool IsTaxable
    );
}
