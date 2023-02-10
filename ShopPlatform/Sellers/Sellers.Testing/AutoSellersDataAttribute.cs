using AutoFixture;
using AutoFixture.Xunit2;

namespace Sellers;

public sealed class AutoSellersDataAttribute : AutoDataAttribute
{
    public AutoSellersDataAttribute()
        : base(() => new Fixture()
            .Customize(new CompositeCustomization(
                new ShopCustomization(), 
                new SellersServerCustomization(),
                new PasswordHasherCustomization())))
    {
    }
}