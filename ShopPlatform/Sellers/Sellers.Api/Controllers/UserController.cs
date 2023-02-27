using System.Collections.Immutable;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Sellers.CommandModel;
using Sellers.Commands;
using Sellers.Filters;
using Sellers.QueryModel;

namespace Sellers.Controllers;

[Route("api/users")]
public class UserController: Controller
{
    [HttpPost("verify-password")]
    public async Task<IActionResult> VerifyPassword(
        [FromBody] Credentials credentials,
        [FromServices] PasswordVerifier verifier)
    {
        (string username, string password) = credentials;
        return await verifier.VerifyPassword(username, password) switch
        {
            true => Ok(),
            _ => BadRequest(),
        };
    }
    
    [HttpPost("{id}/create-user")]
    [TypeFilter(typeof(InvariantViolationFilter))]
    public Task CreateUser(
        Guid id, 
        [FromBody] CreateUser command,
        [FromServices] CreateUserCommandExecutor executor
    )
    {
        return executor.Execute(id, command);
    }

    [HttpPost("{id}/grant-role")]
    [TypeFilter(typeof(EntityNotFoundFilter))]
    public Task GrantRole(
        Guid id,
        [FromBody] GrantRole grantRole,
        [FromServices] GrantRoleCommandExecutor executor
        )
    {
        return executor.Execute(id, grantRole);
    }

    [HttpGet("{id}/roles")]
    public async Task<IActionResult> GetRoles(
        Guid id,
        [FromServices] IUserReader reader)
    {
        return await reader.FindUser(id) is User user 
            ? Ok(user.Roles) 
            : NotFound();
    }
}