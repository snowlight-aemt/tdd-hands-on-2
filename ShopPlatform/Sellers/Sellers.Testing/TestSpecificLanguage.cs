using System.Collections.Immutable;
using System.Net.Http.Json;
using Sellers.Commands;

namespace Sellers;

public static class TestSpecificLanguage
{
    public static async Task<Shop> CreateShop(this SellersServer server, string? name = null)
    {
        HttpClient client = server.CreateClient();
        string url = $"api/shops";
        var body = new { Name = name ?? $"{Guid.NewGuid()}" };
        HttpResponseMessage response = await client.PostAsJsonAsync(url, body);
        return (await response.Content.ReadFromJsonAsync<Shop>())!;
    }
    
    public static Task SetShopUser(this SellersServer server, Guid shopId, string userId, string password)
    {
        string url = $"api/shops/{shopId}/user";
        ShopUser body = new (userId, password);
        return server.CreateClient().PostAsJsonAsync(url, body);
    }

    public static async Task<ShopView?> GetShop(this SellersServer server, Guid shopId)
    {
        string url = $"api/shops/{shopId}";
        HttpResponseMessage response = await server.CreateClient().GetAsync(url);
        HttpContent content = response.EnsureSuccessStatusCode().Content;
        return await content.ReadFromJsonAsync<ShopView>();
    }
    
    public static async Task CreateUser(this SellersServer server, Guid userId, string username, string password)
    {
        HttpClient client = server.CreateClient();
        CreateUser createUser = new (username, password);
        string uri = $"api/users/{userId}/create-user";
        
        await client.PostAsJsonAsync(uri, createUser);
    }
    
    public static async Task<HttpResponseMessage> GrantRole(this SellersServer server, Guid userId, Guid shopId, string roleName)
    {
        HttpClient client = server.CreateClient();
        GrantRole grantRole = new (shopId, roleName);
        string requestUri = $"api/users/{userId}/grant-role";
        
        return await client.PostAsJsonAsync(requestUri, grantRole);
    }

    public static async Task<ImmutableArray<Role>> GetRoles(this SellersServer server, Guid userId)
    {
        HttpResponseMessage response = await server.CreateClient().GetAsync($"api/users/{userId}/roles");
        return await response.Content.ReadFromJsonAsync<ImmutableArray<Role>>();
    }
    
    public static async Task<HttpResponseMessage> RemoveRole(this SellersServer server, Guid userId, Guid shopId, string roleName)
    {
        HttpClient client = server.CreateClient();
        string requestUri = $"api/users/{userId}/remove-role";
        RevokeRole body = new (shopId, roleName);
        
        return await client.PostAsJsonAsync(requestUri, body);
    }
}