using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Orders.Events;
using Polly;
using Polly.Retry;
using Xunit;

namespace Orders.api.orders.accept.payment_approved;

public class Post_specs
{
    [Fact]
    public async Task Sut_returns_Accepted_status_code()
    {
        // arrange
        HttpClient client = OrdersServer.Create().CreateClient();

        string uri = "api/orders/accept/payment-approved";
        
        string tid = $"{Guid.NewGuid()}";
        DateTime approved_at = DateTime.UtcNow;
        ExternalPaymentApproved body = new (tid, approved_at);
        
        // act
        HttpResponseMessage response = await client.PostAsJsonAsync(uri, body);

        // assert
        // 직접 처리하지 않고 메시지를 받아들이고 처리하기 때문에 Accepted
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
    }

    [Fact]
    public async Task Sut_correctly_change_order_status()
    {
        // arranged
        OrdersServer server = OrdersServer.Create();

        Guid ordersId = Guid.NewGuid();
        string paymentTransactionId = $"{Guid.NewGuid()}";
        await server.PlaceOrder(ordersId);
        await server.StartOrder(ordersId, paymentTransactionId);
        ExternalPaymentApproved body = new (
            tid: paymentTransactionId, 
            DateTime.UtcNow);

        // act
        string uri = "api/orders/accept/payment-approved";
        await server.CreateClient().PostAsJsonAsync(uri, body);
        
        // assert
        // 비동기를 테스트 하기 위해서 retry 를 사용
        IAsyncPolicy policy = DefaultPolicy.Instance;
        await policy.ExecuteAsync(async () =>
        {
            Order? order = await server.FindOrder(ordersId);
            order!.Status.Should().Be(OrderStatus.AwaitingShipment);
        });
    }
    

    [Fact]
    public async Task Sut_correctly_change_event_time()
    {
        // arranged
        OrdersServer server = OrdersServer.Create();

        Guid ordersId = Guid.NewGuid();
        string paymentTransactionId = $"{Guid.NewGuid()}";
        await server.PlaceOrder(ordersId);
        await server.StartOrder(ordersId, paymentTransactionId);
        ExternalPaymentApproved body = new (
            tid: paymentTransactionId, 
            DateTime.UtcNow);

        // act
        string uri = "api/orders/accept/payment-approved";
        await server.CreateClient().PostAsJsonAsync(uri, body);
        
        // assert
        // 비동기를 테스트 하기 위해서 retry 를 사용
        IAsyncPolicy policy = DefaultPolicy.Instance;
        await policy.ExecuteAsync(async () =>
        {
            Order? order = await server.FindOrder(ordersId);
            order!.PaidAtUtc.Should().BeCloseTo(body.approved_at, precision: TimeSpan.FromMilliseconds(1));
        });
    }
}