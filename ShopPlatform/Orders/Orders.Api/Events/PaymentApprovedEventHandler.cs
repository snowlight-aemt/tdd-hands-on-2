using Microsoft.EntityFrameworkCore;
using Orders.Messaging;

namespace Orders.Events;

public static class PaymentApprovedEventHandler
{ 
    public static void Listen(IServiceProvider services)
    {
        var stream = services.GetRequiredService<IAsyncObservable<PaymentApproved>>();
        stream.Subscribe(async listenedEvent =>
        {
            IServiceScope scope = services.CreateScope();
            
            var context = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
            
            IQueryable<Order> query =
                from x in context.Orders
                where x.PaymentTransactionId == listenedEvent.PaymentTransactionId
                select x;

            if (await query.SingleOrDefaultAsync() is Order order)
            {
                order.PaidAtUtc = listenedEvent.EventTimeUtc;
                order.Status = OrderStatus.AwaitingShipment;
                // 비동기 테스트를 위해서 강제로 딜레이 시킨다.
                Thread.Sleep(1000);
                await context.SaveChangesAsync();
            }
        });
    }
}