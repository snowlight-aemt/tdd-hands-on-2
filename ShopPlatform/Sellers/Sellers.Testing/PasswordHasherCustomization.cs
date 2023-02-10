using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Sellers.QueryModel;

namespace Sellers;

public sealed class PasswordHasherCustomization: ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register<IPasswordHasher<object>>(() => new PasswordHasher<object>());
    }
    
}