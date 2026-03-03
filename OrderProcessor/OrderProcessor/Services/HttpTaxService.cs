using System.Net.Http.Json;
using System.Text.Json.Serialization;
using OrderProcessor.Models;

namespace OrderProcessor.Services;

public sealed class HttpTaxService : ITaxService
{
    private readonly HttpClient _httpClient;

    public HttpTaxService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

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

    private sealed record TaxResponse(
        [property: JsonPropertyName("is_taxable")]
        bool IsTaxable
    );
}
