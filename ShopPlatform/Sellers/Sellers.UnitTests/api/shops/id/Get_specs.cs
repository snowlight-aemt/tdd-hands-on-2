using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Sellers.api.shops.id;

public class Get_specs
{
    [Fact]
    public async Task Sut_does_not_expose_user_credentials()
    {
        SellersServer server = SellersServer.Create();

        IServiceScope scope = server.Services.CreateScope();
        SellersDbContext context = scope.ServiceProvider.GetRequiredService<SellersDbContext>();
        Shop shop = new()
        {
            Id = Guid.NewGuid(),
            Name = $"{Guid.NewGuid()}",
            UserId = $"{Guid.NewGuid()}",
            PasswordHash = $"{Guid.NewGuid()}",
        };
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