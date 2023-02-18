using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Sellers.Commands;
using Xunit;

namespace Sellers.api.users.id;

public class Post_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_correctly_creates_user(
        SellersServer server,
        CreateUser createUser,
        Guid userId)
    {
        HttpClient client = server.CreateClient();
        string commandUri = $"api/users/{userId}/create-user";
        await client.PostAsJsonAsync(commandUri, createUser);

        Credentials credentials = new (createUser.Username, createUser.Password);
        string queryUri = "api/users/verify-password";
        HttpResponseMessage response = await client.PostAsJsonAsync(queryUri, credentials);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Theory, AutoSellersData]
    public async Task Sut_returns_BadRequest_with_duplicate_username(
        SellersServer server,
        CreateUser createUser,
        Guid userId)
    {
        // arrange
        HttpClient client = server.CreateClient();
        await client.PostAsJsonAsync($"api/users/{userId}/create-user", createUser);
        
        // act
        string uri = $"api/users/{userId}/create-user";
        HttpResponseMessage response = await client.PostAsJsonAsync(uri, createUser);
        
        // assert  
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}