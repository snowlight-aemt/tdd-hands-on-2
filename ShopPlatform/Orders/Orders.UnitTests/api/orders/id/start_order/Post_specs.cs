using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;

namespace Orders.api.orders.id.start_order;

public class Post_specs
{
    [Fact]
    public async void Sut_returns_BadRequest_if_order_already_started()
    {
        //arrange
        OrdersServer server = OrdersServer.Create();
        Guid orderId = Guid.NewGuid();
        await server.PlaceOrder(orderId);
        await server.StartOrder(orderId);

        //act
        HttpResponseMessage response = await server.StartOrder(orderId);

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Sut_returns_BadRequest_if_order_payment_completed()
    {
        //arrange
        OrdersServer server = OrdersServer.Create();
        Guid orderId = Guid.NewGuid();

        await server.PlaceOrder(orderId);
        await server.StartOrder(orderId);
        await server.BankTransferPaymentCompleted(orderId);

        //act
        HttpResponseMessage response = await server.StartOrder(orderId);

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Sut_returns_BadRequest_if_order_completed()
    {
        //arrange
        OrdersServer server = OrdersServer.Create();
        Guid orderId = Guid.NewGuid();

        await server.PlaceOrder(orderId);
        await server.StartOrder(orderId);
        await server.BankTransferPaymentCompleted(orderId);
        await server.HandleItemShipped(orderId);

        //act
        HttpResponseMessage response = await server.StartOrder(orderId);

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Sut_correctly_sets_payment_transaction_id()
    {
        OrdersServer server = OrdersServer.Create();
        Guid ordersId = Guid.NewGuid();
        string? paymentTransactionId = $"{Guid.NewGuid()}";
        await server.PlaceOrder(ordersId);

        await server.StartOrder(ordersId, paymentTransactionId);

        Order? order = await server.FindOrder(ordersId);
        order!.PaymentTransactionId.Should().Be(paymentTransactionId);
    }
}