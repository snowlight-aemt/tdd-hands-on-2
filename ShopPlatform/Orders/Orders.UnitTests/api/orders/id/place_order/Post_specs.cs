using System.Net;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Sellers;
using Xunit;

namespace Orders.api.orders.id.place_order;

public class Post_specs
{
    [Fact]
    public async Task Sut_returns_BadRequest_if_shop_not_exist()
    {
        OrdersServer server = OrdersServer.Create();
        HttpResponseMessage response = await server.PlaceOrder(Guid.NewGuid());
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Sut_returns_Ok_if_shop_exist()
    {
        OrdersServer ordersServer = OrdersServer.Create();
        SellersServer sellersServer = ordersServer.Services.GetRequiredService<SellersServer>();
        Shop shop = await sellersServer.CreateShop();

        HttpResponseMessage response = await ordersServer.PlaceOrder(orderId: Guid.NewGuid(), shop.Id);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}