using System.Collections.Immutable;
using Sellers.Commands;
using Sellers.QueryModel;

namespace Sellers.CommandModel;

public class CreateUserCommandExecutor
{
    private readonly IUserReader reader;
    private readonly IPasswordHasher hasher;
    private readonly IUserRepository repository;

    public CreateUserCommandExecutor(
        IUserReader reader,
        IPasswordHasher hasher,
        IUserRepository repository)
    {
        this.reader = reader;
        this.hasher = hasher;
        this.repository = repository;
    }

    public async Task Execute(Guid id, CreateUser command)
    {
        await AssertThatUsernameIsUnique(command.Username);
        await AddUser(id, command);
    }

    private Task AddUser(Guid id, CreateUser command)
    {
        string hashPassword = this.hasher.HashPassword(command.Password);
        return this.repository.Add(new User(id, command.Username, hashPassword, ImmutableArray<Role>.Empty));
    }

    private async Task AssertThatUsernameIsUnique(string username)
    {
        if (await this.reader.FindUser(username) is not null)
        {
            throw new InvariantViolationException();
        }
    }
}