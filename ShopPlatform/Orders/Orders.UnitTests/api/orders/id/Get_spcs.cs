using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Sellers;
using Xunit;

namespace Orders.api.orders.id;

public class Get_spcs
{
    [Fact]
    public async Task Sut_correctly_sets_shop_name()
    {
        OrdersServer ordersServer = OrdersServer.Create();
        
        Guid orderId = Guid.NewGuid();
        await ordersServer.PlaceOrder(orderId);
        
        Order? order = await ordersServer.FindOrder(orderId);
        ShopView? shop = await ordersServer.GetSellersServer().GetShop(order!.ShopId);
        
        order!.ShopName.Should().Be(shop!.Name);
    }
}