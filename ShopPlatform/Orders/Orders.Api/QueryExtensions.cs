using Microsoft.EntityFrameworkCore;

namespace Orders;

public static class QueryExtensions
{
    public static Task<Order?> FindOrder(this DbSet<Order> source, Guid id)
        => source.SingleOrDefaultAsync(x => x.Id == id);

    public static IQueryable<Order> FilterByUser(this IQueryable<Order> source, Guid? userId)
    {
        return userId is null ? source : source.Where(x => x.UserId == userId);
    }
    
    public static IQueryable<Order> FilterByShop(this IQueryable<Order> source, Guid? shopId)
    {
        return shopId is null ? source : source.Where(x => x.ShopId == shopId);
    }
}
