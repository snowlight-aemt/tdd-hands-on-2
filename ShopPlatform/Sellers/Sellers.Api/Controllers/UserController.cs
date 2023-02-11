using System.Net;
using Microsoft.AspNetCore.Mvc;
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
}