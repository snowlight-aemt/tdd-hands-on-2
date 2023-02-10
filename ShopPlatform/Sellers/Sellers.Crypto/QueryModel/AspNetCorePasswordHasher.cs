using Microsoft.AspNetCore.Identity;

namespace Sellers.QueryModel;

public class AspNetCorePasswordHasher : IPasswordHasher
{
    private IPasswordHasher<object> hasher;
    
    public AspNetCorePasswordHasher(IPasswordHasher<object> hasher)
    {
        this.hasher = hasher;
    }

    public string HashPassword(string password)
    {
        return $"{Guid.NewGuid()}";
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        return true;
    }
}