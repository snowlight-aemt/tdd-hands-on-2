using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace Sellers.api.users;

public class Post_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_returns_BedRequest_with_invalid_credentials(
        SellersServer server,
        Credentials credentials)
    {
        HttpClient client = server.CreateClient();
        string uri = "api/users/verify-password";
        
        HttpResponseMessage response = await client.PostAsJsonAsync(uri, credentials);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Theory, AutoSellersData]
    public async Task Sut_returns_BedRequest_with_valid_credentials(
        SellersServer server,
        string username,
        string password)
    {
        // Arrange
        Shop shop = await server.CreateShop();
        await server.SetShopUser(shop.Id, username, password);
        
        HttpClient client = server.CreateClient();
        string uri = "api/users/verify-password";
        Credentials credentials = new Credentials(username, password);
        
        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync(uri, credentials);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}