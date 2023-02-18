using FluentAssertions;
using Xunit;

namespace Sellers.QueryModel;

public class BackwardCompatibleUserReader_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_returns_correctly_entity_from_user_data_model(
        Func<SellersDbContext> contextFactory,
        UserEntity user,
        BackwardCompatibleUserReader userReader)
    {
        SellersDbContext context = contextFactory.Invoke();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        User? actual = await userReader.FindUser(user.Username);
        actual!.Should().BeEquivalentTo(user, x => x.ExcludingMissingMembers());
    }
    
    [Theory, AutoSellersData]
    public async Task Sut_returns_correctly_entity_from_shop_data_model(
        Func<SellersDbContext> contextFactory,
        Shop shop,
        BackwardCompatibleUserReader userReader)
    {
        // arrange
        SellersDbContext context = contextFactory.Invoke();
        context.Shops.Add(shop);
        await context.SaveChangesAsync();
        string username = shop.UserId;

        // act
        User? actual = await userReader.FindUser(username);
        
        // assert
        User? expected = await new ShopUserReader(contextFactory).FindUser(username);
        actual!.Should().BeEquivalentTo(expected);
    }
}