using Microsoft.EntityFrameworkCore;

namespace Sellers.QueryModel;

public sealed class BackwardCompatibleUserReader: IUserReader
{
    private readonly SqlUserReader defaultReader;
    private readonly ShopUserReader fallback;

    public BackwardCompatibleUserReader(Func<SellersDbContext> contextFactory) 
        : this(
            new SqlUserReader(contextFactory),
            new ShopUserReader(contextFactory))
    {
    }

    private BackwardCompatibleUserReader(SqlUserReader defaultReader, ShopUserReader fallback)
    {
        this.defaultReader = defaultReader;
        this.fallback = fallback;
    }

    public async Task<User?> FindUser(string username) =>
        await this.defaultReader.FindUser(username) 
        ?? await this.fallback.FindUser(username);

    public Task<User?> FindUser(Guid id)
    {
        return Task.FromResult<User?>(null);
    }
}