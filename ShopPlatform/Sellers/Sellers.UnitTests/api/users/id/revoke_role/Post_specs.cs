using System.Collections.Immutable;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Sellers.CommandModel;
using Sellers.Commands;
using Xunit;

namespace Sellers.api.users.id.revoke_role;

public class Post_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_currently_removes_role(
        SellersServer server,
        Guid userId,
        string username,
        string password,
        Guid shopId,
        string roleName)
    {
        await server.CreateUser(userId, username, password);

        await server.GrantRole(userId, shopId, roleName);

        await server.RemoveRole(userId, shopId, roleName);

        ImmutableArray<Role> actual = await server.GetRoles(userId);
        actual.Should().BeEmpty();
    }
    
    [Theory, AutoSellersData]
    public async Task Sut_currently_returns_Not_Found_status_code_if_no_exists(
        SellersServer server,
        Guid userId,
        Guid shopId,
        string roleName)
    {
        HttpResponseMessage response = await server.RemoveRole(userId, shopId, roleName);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}