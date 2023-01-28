using System.Net;
using FluentAssertions;
using Xunit;

namespace Orders.api.orders.handle.bank_transfer_payment_completed;

public class Post_spec
{
    [Fact]
    public async Task Sut_returns_BadRequest_if_order_not_started()
    {
        // arrange
        OrdersServer server = OrdersServer.Create();
        Guid orderId = Guid.NewGuid();
        await server.PlaceOrder(orderId);
        
        // act
        HttpResponseMessage response = await server.BankTransferPaymentCompleted(orderId);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Sut_returns_BadRequest_if_payment_already_completed()
    {
        // arrange
        OrdersServer server = OrdersServer.Create();

        Guid orderId = Guid.NewGuid();
        
        await server.PlaceOrder(orderId);
        await server.StartOrder(orderId);
        await server.BankTransferPaymentCompleted(orderId);
        
        // act
        HttpResponseMessage response = await server.BankTransferPaymentCompleted(orderId);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Sut_returns_BadRequest_if_order_completed()
    {
        // arrange
        OrdersServer server = OrdersServer.Create();

        Guid orderId = Guid.NewGuid();

        await server.PlaceOrder(orderId);
        await server.StartOrder(orderId);
        await server.BankTransferPaymentCompleted(orderId);
        await server.HandleItemShipped(orderId);
        
        // act
        HttpResponseMessage response = await server.BankTransferPaymentCompleted(orderId);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}