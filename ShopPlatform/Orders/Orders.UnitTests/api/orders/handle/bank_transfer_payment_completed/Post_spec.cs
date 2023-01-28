using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Orders.Commands;
using Orders.Events;
using Xunit;

namespace Orders.api.orders.handle.bank_transfer_payment_completed;

public class Post_spec
{
    [Fact]
    public async Task Sut_returns_BadRequest_if_order_not_started()
    {
        // arrange
        HttpClient client = OrdersServer.Create().CreateClient();

        Guid orderId = Guid.NewGuid();
        PlaceOrder placeOrder = new (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 100000);
        await client.PostAsJsonAsync($"api/orders/{orderId}/place-order", placeOrder);
        
        // act
        BankTransferPaymentCompleted bankTransferPaymentCompleted = new (orderId, EventTimeUtc: DateTime.UtcNow);
        string paymentUri = "api/orders/handle/bank-transfer-payment-completed";
        HttpResponseMessage response = await client.PostAsJsonAsync(paymentUri, bankTransferPaymentCompleted);
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}