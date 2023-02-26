using AutoFixture;
using Sellers.CommandModel;

namespace Sellers;

public class UserRepositoryCustomization : ICustomization
{
    public void Customize(IFixture fixture) 
        => fixture.Register<IUserRepository>(() => fixture.Create<SqlUserRepository>());
}