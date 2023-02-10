using FluentAssertions;
using Xunit;

namespace Sellers.QueryModel;

public class ShopUserReader_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_returns_user_entity_matching(
        Func<SellersDbContext> contextFactory,
        Shop shop,
        ShopUserReader sut)
    {
        using SellersDbContext context = contextFactory.Invoke();
        context.Shops.Add(shop);
        await context.SaveChangesAsync();

        User? actual = await sut.FindUser(username: shop.UserId);
        actual.Should().NotBeNull();
        actual!.Username.Should().Be(shop.UserId);
        actual!.PasswordHash.Should().Be(shop.PasswordHash);
        
    }
}