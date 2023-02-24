using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Sellers.Commands;
using Xunit;

namespace Sellers.api.users.id.roles;

public class Get_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_return_NotFound_with_nonexistent_id(
        SellersServer server,
        Guid userId)
    {
        string uri = $"api/users/{userId}/roles";
        HttpResponseMessage response = await server.CreateClient().GetAsync(uri);
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory, AutoSellersData]
    public async Task Sut_returns_OK_with_existing_id(
        SellersServer server,
        ShopUser shopUser)
    {
        HttpClient client = server.CreateClient();
        Shop shop = await server.CreateShop();
        await server.SetShopUser(shop.Id, shopUser.Id, shopUser.Password);

        string uri = $"api/users/{shop.Id}/roles";
        HttpResponseMessage response = await client.GetAsync(uri);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Theory, AutoSellersData]
    public async Task Sut_returns_roles(
        SellersServer server,
        ShopUser shopUser)
    {
        HttpClient client = server.CreateClient();
        Shop shop = await server.CreateShop();
        await server.SetShopUser(shop.Id, shopUser.Id, shopUser.Password);

        string uri = $"api/users/{shop.Id}/roles";
        HttpResponseMessage response = await client.GetAsync(uri);
        
        Role[]? actual = await response.Content.ReadFromJsonAsync<Role[]>();
        Role administrator = new (shop.Id, "Administrator");
        actual!.Should().BeEquivalentTo(new[] {administrator});
    }
}