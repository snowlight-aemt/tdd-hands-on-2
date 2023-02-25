using AutoFixture;
using Microsoft.EntityFrameworkCore;

namespace Sellers;

public sealed class SellersDbContextCustomization: ICustomization
{
    public void Customize(IFixture fixture)
    {
        Func<SellersDbContext> factory = SellersDatabase.GetContext;
        fixture.Register(() => factory);
    }
}