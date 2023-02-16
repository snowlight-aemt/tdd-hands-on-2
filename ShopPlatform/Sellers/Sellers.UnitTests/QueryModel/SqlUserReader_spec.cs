using FluentAssertions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sellers.CommandModel;
using Xunit;

namespace Sellers.QueryModel;

public class SqlUserReader_spec
{
    [Theory, AutoSellersData]
    public async Task Sut_returns_entity_with_matching_name (
        Func<SellersDbContext> contextFactory,
        SqlUserReader userReader,
        UserEntity user )
    {
        using SellersDbContext context = contextFactory.Invoke();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        User? actual = await userReader.FindUser(user.Username);

        actual!.Should().NotBeNull(); 
        actual!.Should().BeEquivalentTo(user, c => c.ExcludingMissingMembers());
    }

}