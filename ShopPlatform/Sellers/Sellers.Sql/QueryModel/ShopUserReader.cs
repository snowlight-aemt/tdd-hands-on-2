using System.Collections.Immutable;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Sellers.QueryModel;

public sealed class ShopUserReader: IUserReader
{
    private readonly Func<SellersDbContext> contextFactory;

    public ShopUserReader(Func<SellersDbContext> contextFactory) 
        => this.contextFactory = contextFactory;

    public Task<User?> FindUser(string username) => FindUser(x => x.UserId == username);

    public Task<User?> FindUser(Guid id) => FindUser(x => x.Id == id);

    private async Task<User?> FindUser(Expression<Func<Shop, bool>> predicate)
    {
        using SellersDbContext context = contextFactory.Invoke();
        IQueryable<Shop> query = context.Shops.AsNoTracking().Where(predicate);

        return await query.SingleOrDefaultAsync() switch
        {
            Shop shop => Translate(shop),
            _ => null,
        };
    }

    private static User? Translate(Shop shop)
    {
        ImmutableArray<Role> administrator = ImmutableArray.Create(new Role(shop.Id, "Administrator"));
        return new User(shop.Id, shop.UserId, shop.PasswordHash, administrator);
    }
}