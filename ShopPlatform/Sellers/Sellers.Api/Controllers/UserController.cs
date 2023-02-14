using System.Net;
using Microsoft.AspNetCore.Mvc;
using Sellers.CommandModel;
using Sellers.Commands;
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
    public void CreateUser(
        Guid id, 
        [FromBody] CreateUser command,
        [FromServices] CreateUserCommandExecutor executor
    )
    {
        
    }
}