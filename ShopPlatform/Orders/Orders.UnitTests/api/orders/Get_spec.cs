using System.Net.Http.Json;
using FluentAssertions;
using Orders.Commands;
using Xunit;

namespace Orders.api.orders;

public class Get_spec
{
    [Fact]
    public async void Sut_correctly_applies_user_filter()
    {
        // Arrange
        OrdersServer server = OrdersServer.Create();
        HttpClient client = server.CreateClient();

        Guid shopId = Guid.NewGuid();
        Guid itemId = Guid.NewGuid();
        decimal price = 100000;
        
        List<PlaceOrder> commends = new()
        {
            new PlaceOrder(Guid.NewGuid(), shopId, itemId, price),
            new PlaceOrder(Guid.NewGuid(), shopId, itemId, price),
            new PlaceOrder(Guid.NewGuid(), shopId, itemId, price),
        };
        
        await Task.WhenAll(from commend in commends
                            let id = Guid.NewGuid()
                            let commandUri = $"api/orders/{id}/place-order"
                            select client.PostAsJsonAsync(commandUri, commend));

        Guid userId = commends[0].UserId;
        
        // Act
        string queryUri = $"api/orders?user-id={userId}";
        HttpResponseMessage response = await client.GetAsync(queryUri);
        Order[]? orders = await response.Content.ReadFromJsonAsync<Order[]>();
        
        // Assert
        orders!.Should().OnlyContain(o => o.UserId.Equals(userId));
    } 
}