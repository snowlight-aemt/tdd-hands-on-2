using System.Collections.Immutable;
using Sellers.Commands;

namespace Sellers;

public sealed record User(Guid Id, string Username, string PasswordHash, ImmutableArray<Role> Roles)
{
    public User GrantRole(GrantRole command)
    {
        Role role = new (command.ShopId, command.RoleName);
        return this with { Roles = Roles.Add(role) };
    }
}