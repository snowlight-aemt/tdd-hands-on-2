using System.Collections.Immutable;
using System.Net;
using System.Net.Http.Json;
using AutoMapper;
using FluentAssertions;
using Sellers.Commands;
using Xunit;

namespace Sellers.api.users.id.grant_role;

public class Post_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_returns_Ok(
        SellersServer server,
        CreateUser createUser,
        GrantRole grantRole,
        Guid userId)
    {
        HttpClient client = server.CreateClient();
        await client.PostAsJsonAsync($"api/users/{userId}/create-user", createUser);

        string uri = $"api/users/{userId}/grant-role";
        HttpResponseMessage response = await client.PostAsJsonAsync(uri, grantRole);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Theory, AutoSellersData]
    public async Task Sut_returns_NotFound_status_code_if_user_not_exists(
        SellersServer server,
        Guid shopId,
        Guid userId,
        string roleName)
    {
        HttpResponseMessage response = await server.GrantRole(shopId, userId, roleName);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory, AutoSellersData]
    public async Task Sut_currently_adds_role(
        SellersServer server,
        Guid userId,
        string username,
        string password,
        Guid shopId,
        string roleName)
    {
        await server.CreateUser(userId, username, password);

        await server.GrantRole(userId, shopId, roleName);

        ImmutableArray<Role> actual = await server.GetRoles(userId);
        actual.Should().Contain(new Role(shopId, roleName));
    }
}