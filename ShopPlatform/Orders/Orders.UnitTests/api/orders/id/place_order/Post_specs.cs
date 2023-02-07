using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Net.Http.Json;
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
        Guid shopId = Guid.NewGuid();
        Guid orderId = Guid.NewGuid();
        HttpResponseMessage response = await server.PlaceOrder(orderId, shopId);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Sut_returns_Ok_if_shop_exist()
    {
        OrdersServer ordersServer = OrdersServer.Create();
        HttpResponseMessage response = await ordersServer.PlaceOrder(orderId: Guid.NewGuid());
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Sut_does_not_create_order_if_shop_not_exists()
    {
        OrdersServer ordersServer = OrdersServer.Create();

        Guid shopId = Guid.NewGuid();
        Guid orderId = Guid.NewGuid();
        await ordersServer.PlaceOrder(orderId, shopId);

        HttpResponseMessage response = await ordersServer.CreateClient().GetAsync($"api/orders/{orderId}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

    }
}