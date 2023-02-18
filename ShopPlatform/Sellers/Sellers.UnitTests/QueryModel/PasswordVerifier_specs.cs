using FluentAssertions;
using Xunit;

namespace Sellers.QueryModel;

public class PasswordVerifier_specs
{
    [Theory]
    [InlineAutoSellersData("hello word", "hello word", true)]
    [InlineAutoSellersData("hello word", "yellow word", false)]
    public async Task VerifyPassword_workds_correctly(
        string password, 
        string providedPassword, 
        bool result,
        Func<SellersDbContext> contextFactory,
        ShopUserReader reader,
        IPasswordHasher hasher, 
        Shop shop)
    {
        using SellersDbContext context = contextFactory.Invoke();
        shop.PasswordHash = hasher.HashPassword(password);
        context.Shops.Add(shop);
        await context.SaveChangesAsync();
        PasswordVerifier passwordVerifier = new (reader, hasher);
        
        bool actual = await passwordVerifier.VerifyPassword(shop.UserId, providedPassword);
        actual.Should().Be(result);
    }

    [Theory, AutoSellersData]
    public async Task verifyPassword_returns_false_with_non_existent_username(
        ShopUserReader reader,
        IPasswordHasher hasher,
        string username,
        string password)
    {
        PasswordVerifier passwordVerifier = new (reader, hasher);
        bool actual = await passwordVerifier.VerifyPassword(username, password);

        actual.Should().BeFalse();
    }
}