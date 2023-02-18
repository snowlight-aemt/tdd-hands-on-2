using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Sellers.QueryModel;
using Xunit;

namespace Sellers;

public class Program_specs
{
    [Theory, AutoSellersData]
    public void Sut_register_IUserReader_service_with_BackwardCompatibleUserReader(
        SellersServer server)
    {
        IUserReader sut = server.Services.GetRequiredService<IUserReader>();
        sut.Should().BeOfType<BackwardCompatibleUserReader>();
    }
}