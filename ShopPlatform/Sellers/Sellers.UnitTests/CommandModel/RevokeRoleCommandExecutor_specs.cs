using FluentAssertions;
using Sellers.Commands;
using Sellers.QueryModel;
using Xunit;

namespace Sellers.CommandModel;

public class RevokeRoleCommandExecutor_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_revkes_role(
        RevokeRoleCommandExecutor sut,
        Func<SellersDbContext> contextFactory,
        UserEntity user,
        Role role)
    {
        SellersDbContext context = contextFactory.Invoke();
        user.Roles.Add(new RoleEntity() {ShopId = role.ShopId, RoleName = role.RoleName});
        context.Users.Add(user);
        await context.SaveChangesAsync();

        await sut.Execute(user.Id, new RevokeRole(role.ShopId, role.RoleName));

        User? actual = await new SqlUserReader(contextFactory).FindUser(user.Id);
        actual!.Roles.Should().NotContain(role);
    }
    
    [Theory, AutoSellersData]
    public async Task Sut_removes_only_specified_role(
        RevokeRoleCommandExecutor sut,
        Func<SellersDbContext> contextFactory,
        UserEntity user,
        Role role1,
        Role role2)
    {
        SellersDbContext context = contextFactory.Invoke();
        user.Roles.Add(new RoleEntity() {ShopId = role1.ShopId, RoleName = role1.RoleName});
        user.Roles.Add(new RoleEntity() {ShopId = role2.ShopId, RoleName = role2.RoleName});
        context.Users.Add(user);
        await context.SaveChangesAsync();

        await sut.Execute(user.Id, new RevokeRole(role1.ShopId, role1.RoleName));
        
        User? actual = await new SqlUserReader(contextFactory).FindUser(user.Id);
        actual!.Roles.Should().Contain(role2);
    }
    
}