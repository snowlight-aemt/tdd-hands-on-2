using FluentAssertions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sellers.CommandModel;
using Xunit;

namespace Sellers.QueryModel;

public class SqlUserReader_spec
{
    [Theory, AutoSellersData]
    public async Task Sut_returns_entity_with_matching_name(
        Func<SellersDbContext> contextFactory,
        SqlUserReader userReader,
        UserEntity user)
    {
        using SellersDbContext context = contextFactory.Invoke();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        User? actual = await userReader.FindUser(user.Username);

        actual!.Should().NotBeNull();
        actual!.Should().BeEquivalentTo(user, c => c.ExcludingMissingMembers());
    }

    [Theory, AutoSellersData]
    public async Task Sut_returns_entity_with_no_matching_name(
        Func<SellersDbContext> contextFactory,
        SqlUserReader userReader,
        UserEntity user)
    {
        User? actual = await userReader.FindUser(user.Username);
        actual!.Should().BeNull();
    }
    
    [Theory, AutoSellersData]
    public async Task Sut_returns_entity_with_matching_id(
        Func<SellersDbContext> contextFactory,
        SqlUserReader userReader,
        UserEntity user)
    {
        using SellersDbContext context = contextFactory.Invoke();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        User? actual = await userReader.FindUser(user.Id);

        actual!.Should().NotBeNull();
        actual!.Should().BeEquivalentTo(user, c => c.ExcludingMissingMembers());
    }

    [Theory, AutoSellersData]
    public async Task Sut_returns_entity_with_no_matching_id(
        Func<SellersDbContext> contextFactory,
        SqlUserReader userReader,
        UserEntity user)
    {
        User? actual = await userReader.FindUser(user.Id);
        actual!.Should().BeNull();
    }
    
    [Theory, AutoSellersData]
    public async Task Sut_returns_role_entities_with_matching_id(
        Func<SellersDbContext> contextFactory,
        SqlUserReader sut,
        UserEntity user,
        Role[] roles)
    {
        // arrange
        using SellersDbContext context = contextFactory.Invoke();
        context.Users.Add(user);
        await context.SaveChangesAsync();
        context.Roles.AddRange(from r in roles
                                select new RoleEntity
                                {
                                    UserSequence = user.Sequence,
                                    ShopId = r.ShopId,
                                    RoleName = r.RoleName
                                });
        await context.SaveChangesAsync();

        // act
        User actual = (await sut.FindUser(user.Id))!;

        // assert
        actual.Roles.Should().BeEquivalentTo(roles);
    }
} 