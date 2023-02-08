using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Sellers.api.shops;

public class Get_specs
{
    private const string connectionString = "Server=127.0.0.1;Port=5432;Database=Sellers_UnitTests_8129;User Id=postgres;Password=mysecretpassword;";
    
    [Theory, AutoSellersData]
    public async Task Sut_returns_all_shops([ConnectionString(connectionString)] SellersServer server, Shop[] shops)
    {
        using IServiceScope scope = server.Services.CreateScope();
        SellersDbContext context = scope.ServiceProvider.GetRequiredService<SellersDbContext>();
        context.Shops.RemoveRange(await context.Shops.ToListAsync());
        context.Shops.AddRange(shops);
        await context.SaveChangesAsync();
        
        HttpResponseMessage response = await server.CreateClient().GetAsync("api/shops");
        Shop[]? actual = await response.Content.ReadFromJsonAsync<Shop[]>();
        actual!.Should().BeEquivalentTo(shops, c => c.Excluding(x => x.Sequence));
    }
}