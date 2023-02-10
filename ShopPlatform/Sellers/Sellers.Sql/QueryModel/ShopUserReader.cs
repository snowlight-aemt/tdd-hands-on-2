namespace Sellers.QueryModel;

public sealed class ShopUserReader: IUserReader
{
    public ShopUserReader(Func<SellersDbContext> contextFactory)
    {
    }
    
    public Task<User?> FindUser(string username)
    {
        return Task.FromResult<User?>(default);
    }
}