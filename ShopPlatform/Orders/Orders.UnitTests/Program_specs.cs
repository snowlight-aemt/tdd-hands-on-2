using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Orders;

public class Program_specs
{
    [Fact]
    public void Sut_correctly_registers_SellersServcie()
    {
        OrdersServer server = OrdersServer.Create();
        SellersService? actual = server.Services.GetService<SellersService>();
        actual.Should().NotBeNull();
    }
}