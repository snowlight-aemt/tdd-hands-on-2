using System.Net.Http.Json;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Sellers.api.shops.id;

public class Get_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_does_not_expose_user_credentials(Shop shop, SellersServer server)
    {
        IServiceScope scope = server.Services.CreateScope();
        SellersDbContext context = scope.ServiceProvider.GetRequiredService<SellersDbContext>();
        
        context.Shops.Add(shop);
        await context.SaveChangesAsync();

        HttpClient client = server.CreateClient();
        HttpResponseMessage response = await client.GetAsync($"api/shops/{shop.Id}");
        Shop? actual = await response.Content.ReadFromJsonAsync<Shop>();

        actual!.UserId.Should().BeNull();
        actual.PasswordHash.Should().BeNull();
    }
    
    [Fact]
    public async Task Sut_does_not_expose_user_information()
    {
        // arrange
        SellersServer server = SellersServer.Create();

        Shop shop = await server.CreateShop();
        await server.SetShopUser(shop.Id, userId: $"{Guid.NewGuid()}", "password 1");
        
        // act
        HttpResponseMessage response = await server.CreateClient().GetAsync($"api/shops/{shop.Id}");
        Shop? actual = await response.Content.ReadFromJsonAsync<Shop>();

        // assert
        actual!.UserId.Should().BeNull();
        actual!.PasswordHash.Should().BeNull();
    }


}