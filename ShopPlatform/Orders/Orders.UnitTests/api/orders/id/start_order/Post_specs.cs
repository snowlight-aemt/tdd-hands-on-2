using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Orders.Commands;
using Xunit;

namespace Orders.api.orders.id.start_order;

public class Post_specs
{
    [Fact]
    public async void Sut_returns_BadRequest_if_order_already_started()
    {
        //arrange
        HttpClient client = OrdersServer.Create().CreateClient();
        Guid orderId = Guid.NewGuid();
        
        PlaceOrder placeOrder = new (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 100000);
        await client.PostAsJsonAsync($"api/orders/{orderId}/place-order", placeOrder);
        
        string startUrl = $"api/orders/{orderId}/start-order";
        StartOrder startOrder = new ();
        await client.PostAsJsonAsync(startUrl, startOrder);
        
        //act
        HttpResponseMessage response = await client.PostAsJsonAsync(startUrl, startOrder);
        
        //assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}