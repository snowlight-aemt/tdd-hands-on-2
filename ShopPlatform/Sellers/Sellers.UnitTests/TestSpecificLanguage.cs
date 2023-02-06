using System.Net.Http.Json;

namespace Sellers;

public static class TestSpecificLanguage
{
    public static async Task<Shop> CreateShop(this SellersServer server)
    {
        HttpClient client = server.CreateClient();
        string url = $"api/shops";
        var body = new { Name = $"{Guid.NewGuid()}" };
        HttpResponseMessage response = await client.PostAsJsonAsync(url, body);
        return (await response.Content.ReadFromJsonAsync<Shop>())!;
    }
    
    public static Task SetShopUser(this SellersServer server, Guid shopId, string userId, string password)
    {
        string url = $"api/shops/{shopId}/user";
        ShopUser body = new (userId, password);
        return server.CreateClient().PostAsJsonAsync(url, body);
    }
}