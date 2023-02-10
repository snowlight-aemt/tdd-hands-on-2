using System.Net;
using Microsoft.AspNetCore.Mvc;
using Sellers.QueryModel;

namespace Sellers.Controllers;

[Route("api/users")]
public class UserController: Controller
{
    [HttpPost("verify-password")]
    public void VerifyPassword(
        [FromBody] Credentials credentials,
        [FromServices] PasswordVerifier verifier)
    {
        
    }
    
}