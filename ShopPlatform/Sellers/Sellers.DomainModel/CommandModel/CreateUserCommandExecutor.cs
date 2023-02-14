using Sellers.Commands;
using Sellers.QueryModel;

namespace Sellers.CommandModel;

public class CreateUserCommandExecutor
{
    public CreateUserCommandExecutor(
        IUserReader reader,
        IPasswordHasher hasher,
        IUserRepository repository)
    {
    }

    public Task Execute(Guid id, CreateUser command)
    {
        return Task.CompletedTask;
    }
}