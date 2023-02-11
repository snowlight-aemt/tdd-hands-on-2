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
}