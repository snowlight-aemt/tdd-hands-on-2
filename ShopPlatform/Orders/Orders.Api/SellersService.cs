using Sellers;

namespace Orders;

public sealed class SellersService
{
    private readonly HttpClient client;

    public SellersService(HttpClient client) => this.client = client;

    public async Task<bool> ShopExists(Guid shopId)
    {
        string uri = $"api/shops/{shopId}";
        HttpResponseMessage response = await this.client.GetAsync(uri);
        return response.IsSuccessStatusCode;
    }

    internal async Task<ShopView> GetShop(Guid shopId)
    {
        string uri = $"api/shops/{shopId}";
        HttpResponseMessage response = await client.GetAsync(uri);
        HttpContent content = response.Content;
        return (await content.ReadFromJsonAsync<ShopView>())!;
    }
}