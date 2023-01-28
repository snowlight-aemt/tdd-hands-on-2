using System.Net.Http.Json;
using Orders.Commands;
using Orders.Events;

namespace Orders;

public static class TestSpecifiedLanguage
{
    public static async Task PlaceOrder(this OrdersServer server, Guid orderId)
    {
        HttpClient client = server.CreateClient();

        string uri = $"api/orders/{orderId}/place-order";
        PlaceOrder placeOrder = new (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 100000);
        await client.PostAsJsonAsync(uri, placeOrder);
    }

    public static async Task<HttpResponseMessage> StartOrder(this OrdersServer server, Guid orderId)
    {
        HttpClient client = server.CreateClient();
        
        string uri = $"api/orders/{orderId}/start-order";
        return await client.PostAsJsonAsync(uri, new StartOrder());
    }
   
    public static async Task<HttpResponseMessage> BankTransferPaymentCompleted(this OrdersServer server, Guid orderId)
    {
        HttpClient client = server.CreateClient();

        string uri = $"api/orders/handle/bank-transfer-payment-completed";
        BankTransferPaymentCompleted bankTransferPaymentCompleted = new(orderId, EventTimeUtc: DateTime.UtcNow);
        return await client.PostAsJsonAsync(uri, bankTransferPaymentCompleted);
    }

    public static async Task<HttpResponseMessage> HandleItemShipped(this OrdersServer server, Guid orderId)
    {
        HttpClient client = server.CreateClient();

        string uri = $"api/orders/handle/item-shipped";
        ItemShipped itemShipped = new (orderId, DateTime.UtcNow);
        return await client.PostAsJsonAsync(uri, itemShipped);
    }
}