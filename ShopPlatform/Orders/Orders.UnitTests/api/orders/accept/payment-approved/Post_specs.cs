using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Orders.Events;
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
}