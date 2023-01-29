using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http.Features;
using Orders.Commands;
using Orders.Events;
using Xunit;

namespace Orders.api.orders.handle.item_shipped;

public class Post_spec
{
    [Fact]
    public async Task Sut_correctly_sets_event_time()
    {
        // Arrange
        OrdersServer server = OrdersServer.Create();
        using HttpClient client = server.CreateClient();

        Guid orderId = Guid.NewGuid();

        await server.PlaceOrder(orderId);
        await server.StartOrder(orderId);
        await server.BankTransferPaymentCompleted(orderId);
        
        // Act
        DateTime eventTimeUtc = DateTime.UtcNow;
        ItemShipped itemShipped = new(orderId, EventTimeUtc:eventTimeUtc);
        await client.PostAsJsonAsync($"api/orders/handle/item-shipped", itemShipped);

        //Assert
        HttpResponseMessage response = await client.GetAsync($"api/orders/{orderId}");
        Order? order = await response.Content.ReadFromJsonAsync<Order>();
        order!.ShippedAtUtc.Should().BeCloseTo(eventTimeUtc, precision: TimeSpan.FromMilliseconds(1));
    }

    [Fact]
    public async Task Sut_returns_BadRequest_if_order_not_started()
    {
        // arrange
        OrdersServer server = OrdersServer.Create();
        Guid orderId = Guid.NewGuid();
        
        await server.PlaceOrder(orderId);
        
        // act
        HttpResponseMessage response = await server.HandleItemShipped(orderId);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Sut_returns_BadRequest_if_order_already_completed()
    {
        // arrange
        OrdersServer server = OrdersServer.Create();
        Guid orderId = Guid.NewGuid();
        
        await server.PlaceOrder(orderId);
        await server.StartOrder(orderId);
        await server.BankTransferPaymentCompleted(orderId);
        await server.HandleItemShipped(orderId);
        
        // act
        HttpResponseMessage response = await server.HandleItemShipped(orderId);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}