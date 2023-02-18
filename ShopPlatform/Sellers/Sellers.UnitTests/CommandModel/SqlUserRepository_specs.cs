using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Sellers.CommandModel;

public class SqlUserRepository_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_correctly_adds_entity(
        SqlUserRepository sut,
        User user,
        Func<SellersDbContext> contextFactory)
    {
        await sut.Add(user);

        using SellersDbContext context = contextFactory.Invoke();
        UserEntity? actual = await context.Users.SingleOrDefaultAsync(x => x.Id == user.Id);
        actual.Should().BeEquivalentTo(user, c => c.ExcludingMissingMembers());
    }
}