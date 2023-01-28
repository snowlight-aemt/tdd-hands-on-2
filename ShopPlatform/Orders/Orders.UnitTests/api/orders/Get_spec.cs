using System.Net.Http.Json;
using FluentAssertions;
using Orders.Commands;
using Xunit;

namespace Orders.api.orders;

public class Get_spec
{
    [Fact]
    public async Task Sut_correctly_applies_user_filter()
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

    [Fact]
    public async Task Sut_correctly_filters_orders_by_shop()
    {
        //arrange
        OrdersServer server = OrdersServer.Create();
        HttpClient client = server.CreateClient();

        Guid userId = Guid.NewGuid();
        Guid itemId = Guid.NewGuid();
        decimal price = 100000;
        List<PlaceOrder> commands = new()
        {
            new PlaceOrder(userId, Guid.NewGuid(), itemId, price),
            new PlaceOrder(userId, Guid.NewGuid(), itemId, price),
            new PlaceOrder(userId, Guid.NewGuid(), itemId, price),
        };

        await Task.WhenAll(from command in commands
                            let id = Guid.NewGuid()
                            let uri = $"api/orders/{id}/place-order"
                            select client.PostAsJsonAsync(uri, command));
        Guid shopId = commands[0].ShopId;
        // act
        string queryUri = $"api/orders?shop-id={shopId}";
        HttpResponseMessage response = await client.GetAsync(queryUri);
        Order[]? orders = await response.Content.ReadFromJsonAsync<Order[]>();

        //assert
        orders!.Should().OnlyContain(o => o.ShopId.Equals(shopId));
    }

    [Fact]
    public async Task Sut_correctly_filters_orders_by_user_and_shop()
    {
        // Arrange
        HttpClient client = OrdersServer.Create().CreateClient();

        Guid userId = Guid.NewGuid();
        Guid shopId = Guid.NewGuid();
        
        List<PlaceOrder> commands = new()
        {
            new PlaceOrder(userId, Guid.NewGuid(), Guid.NewGuid(), 100000),
            new PlaceOrder(userId, shopId, Guid.NewGuid(), 100000),
            new PlaceOrder(Guid.NewGuid(), shopId, Guid.NewGuid(), 100000),
        };

        await Task.WhenAll(from command in commands
                            let id = Guid.NewGuid()
                            let uri = $"api/orders/{id}/place-order"
                            select client.PostAsJsonAsync(uri, command));

        // Act

        HttpResponseMessage response = await client.GetAsync($"api/orders?user-id={userId}&shop-id={shopId}");
        Order[]? orders = await response.Content.ReadFromJsonAsync<Order[]>();
        
        // Assert
        orders.Should().OnlyContain(x => x.UserId.Equals(userId) && x.ShopId.Equals(shopId));
    }
}