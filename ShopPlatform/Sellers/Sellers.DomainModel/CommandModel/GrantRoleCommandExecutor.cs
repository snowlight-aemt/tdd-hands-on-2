using System.Collections.Immutable;
using Sellers.Commands;

namespace Sellers.CommandModel;

public sealed class GrantRoleCommandExecutor
{
    private readonly IUserRepository repository;

    public GrantRoleCommandExecutor(IUserRepository repository) 
        => this.repository = repository;

    public Task Execute(Guid id, GrantRole command)
    {
        return this.repository.TryUpdate(id, user
            => user with { Roles = ImmutableArray.Create(new Role(command.ShopId, command.RoleName)) });
    }
}