using System.Collections.Immutable;
using Sellers.Commands;

namespace Sellers.CommandModel;

public sealed class GrantRoleCommandExecutor
{
    private readonly IUserRepository repository;

    public GrantRoleCommandExecutor(IUserRepository repository) 
        => this.repository = repository;

    public Task Execute(Guid id, GrantRole command) 
        => this.repository.TryUpdate(id, user => user.GrantRole(command));
}