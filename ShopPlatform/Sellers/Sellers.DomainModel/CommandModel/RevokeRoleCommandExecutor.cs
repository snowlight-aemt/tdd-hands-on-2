using System.Collections.Immutable;
using System.Security.AccessControl;
using Sellers.Commands;
using Sellers.QueryModel;

namespace Sellers.CommandModel;

public class RevokeRoleCommandExecutor
{
    private readonly IUserRepository repository;

    public RevokeRoleCommandExecutor(IUserRepository repository)
    {
        this.repository = repository;
    }

    public async Task Execute(Guid id, RevokeRole command)
    {
        if (await this.repository.TryUpdate(id, user => user.RevokeRole(command)) == false)
            throw new EntityNotFoundException();
    }
}