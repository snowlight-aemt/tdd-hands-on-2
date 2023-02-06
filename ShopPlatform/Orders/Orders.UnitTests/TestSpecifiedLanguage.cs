using System.Net.Http.Json;
using Orders.Commands;
using Orders.Events;

namespace Orders;

public static class TestSpecifiedLanguage
{
    public static async Task<HttpResponseMessage> PlaceOrder(this OrdersServer server, Guid orderId, Guid? shopId = null)
    {
        HttpClient client = server.CreateClient();

        string uri = $"api/orders/{orderId}/place-order";
        PlaceOrder placeOrder = new (Guid.NewGuid(), shopId ?? Guid.NewGuid(), Guid.NewGuid(), 100000);
        return await client.PostAsJsonAsync(uri, placeOrder);
    }

    public static async Task<HttpResponseMessage> StartOrder(this OrdersServer server, 
                                                                Guid orderId, string? paymentTransactionId = null)
    {
        HttpClient client = server.CreateClient();
        
        string uri = $"api/orders/{orderId}/start-order";
        StartOrder body = new StartOrder(paymentTransactionId);
        return await client.PostAsJsonAsync(uri, body);
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
    
    public static async Task<Order?> FindOrder(this OrdersServer server, Guid ordersId)
    {
        HttpClient client = server.CreateClient();
        string uri = $"api/orders/{ordersId}";
        HttpResponseMessage response = await client.GetAsync(uri);
        return await response.Content.ReadFromJsonAsync<Order>();
    }
}