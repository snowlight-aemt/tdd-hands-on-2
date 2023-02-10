using Microsoft.EntityFrameworkCore;

namespace Sellers.QueryModel;

public sealed class ShopUserReader: IUserReader
{
    private readonly Func<SellersDbContext> contextFactory;

    public ShopUserReader(Func<SellersDbContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }
    
    public async Task<User?> FindUser(string username)
    {
        using SellersDbContext context = contextFactory.Invoke();
        
        IQueryable<Shop> query = 
                from x in context.Shops
                where x.UserId == username
                select x;
        
        Shop shop = await query.SingleAsync();
        return Translate(shop);
    }

    private static User? Translate(Shop shop)
    {
        return new User(default, shop.UserId, shop.PasswordHash);
    }
}