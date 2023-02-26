using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sellers.Commands;
using Sellers.QueryModel;
using Xunit;

namespace Sellers.CommandModel;

public class GrantRoleCommandExecutor_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_currently_add_role(
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
}