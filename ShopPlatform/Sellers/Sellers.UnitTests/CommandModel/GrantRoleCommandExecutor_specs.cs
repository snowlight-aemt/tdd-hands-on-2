using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sellers.Commands;
using Sellers.QueryModel;
using Xunit;

namespace Sellers.CommandModel;

public class GrantRoleCommandExecutor_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_currently_adds_role(
        GrantRoleCommandExecutor sut,
        Func<SellersDbContext> contextFactory,
        GrantRole command,
        UserEntity user)
    {
        SellersDbContext context = contextFactory.Invoke();
        context.Users.Add(user);
        await context.SaveChangesAsync();
        
        await sut.Execute(user.Id, command);

        User actual = (await new SqlUserReader(contextFactory).FindUser(user.Id))!;
        actual.Roles.Should().NotBeEmpty();
        actual.Roles.Should().Contain(new Role(command.ShopId, command.RoleName));
    }

    public async Task Sut_currently_appends_role(
        GrantRoleCommandExecutor sut,
        Func<SellersDbContext> contextFactory,
        GrantRole command1,
        GrantRole command2,
        UserEntity user)
    {
        SellersDbContext context = contextFactory.Invoke();
        context.Users.Add(user);
        await context.SaveChangesAsync();
        await sut.Execute(user.Id, command1);
        
        await sut.Execute(user.Id, command2);

        User actual = (await new SqlUserReader(contextFactory).FindUser(user.Id))!;
        actual.Roles.Should().Contain(new Role(command1.ShopId, command1.RoleName));
        actual.Roles.Should().Contain(new Role(command2.ShopId, command2.RoleName));
    }
}