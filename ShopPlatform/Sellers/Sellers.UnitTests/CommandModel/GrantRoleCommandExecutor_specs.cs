using System.Configuration;
using AutoFixture.Xunit2;
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

    [Theory, AutoSellersData]
    public async Task Sut_fails_if_user_not_exists(
        GrantRoleCommandExecutor sut,
        Guid userId,
        GrantRole command)
    {
        Func<Task> aciton = () => sut.Execute(userId, command);
        await aciton.Should().ThrowAsync<EntityNotFoundException>();
    }

    [Theory, AutoSellersData]
    public async Task Sut_currently_appends_role(
        InMemoryUserRepository repository,
        GrantRole command1,
        GrantRole command2,
        User user)
    { 
        GrantRoleCommandExecutor sut = new(repository );
        await repository.Add(user);
        await sut.Execute(user.Id, command1);
        
        await sut.Execute(user.Id, command2);

        User actual = repository.Single(u => u.Id == user.Id); 
        actual.Roles.Should().Contain(new Role(command1.ShopId, command1.RoleName));
        actual.Roles.Should().Contain(new Role(command2.ShopId, command2.RoleName));
    }

    public class InMemoryUserRepository : List<User>, IUserRepository
    {
        public Task Add(User user)
        {
            base.Add(user);
            return Task.CompletedTask;
        }

        public Task<bool> TryUpdate(Guid id, Func<User, User> reviser)
        {
            if (this.SingleOrDefault(x => x.Id == id) is User user)
            {
                int index = FindIndex(u => u == user);
                this[index] = reviser.Invoke(user);
                return Task.FromResult(true);       
            }
            else
            {
                return Task.FromResult(false);
            }
        }
    }
}