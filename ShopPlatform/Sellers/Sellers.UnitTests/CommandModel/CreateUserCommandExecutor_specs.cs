using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sellers.Commands;
using Sellers.QueryModel;
using Xunit;

namespace Sellers.CommandModel;

public class CreateUserCommandExecutor_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_correctly_creates_entity(
        SqlUserReader reader,
        IPasswordHasher hasher,
        SqlUserRepository repository,
        CreateUser createUser,
        Guid userId
    )
    {
        CreateUserCommandExecutor sut = new (reader, hasher, repository);
        await sut.Execute(userId, createUser);

        User? actual = await reader.FindUser(createUser.Username);
        actual!.Should().NotBeNull();
        actual!.Id.Should().Be(userId); 
    }
    
    [Theory, AutoSellersData]
    public async Task Sut_correctly_sets_password_hash(
        SqlUserReader reader,
        IPasswordHasher hasher,
        SqlUserRepository repository,
        CreateUser createUser,
        Guid userId
    )
    {
        CreateUserCommandExecutor sut = new (reader, hasher, repository);
        await sut.Execute(userId, createUser);

        User? user = await reader.FindUser(createUser.Username);
        bool actual = hasher.VerifyPassword(user! .PasswordHash, createUser.Password);
        actual.Should().BeTrue();
    }
    
    [Theory, AutoSellersData]
    public async Task Sut_correctly_fails_with_duplicate_username(
        SqlUserReader reader,
        IPasswordHasher hasher,
        SqlUserRepository repository,
        CreateUser createUser
    )
    {
        CreateUserCommandExecutor sut = new (reader, hasher, repository);
        await sut.Execute(Guid.NewGuid(), createUser);
        
        Func<Task> action = () => sut.Execute(Guid.NewGuid(), createUser);
        await action.Should().ThrowAsync<InvariantViolationException>();
    }
}