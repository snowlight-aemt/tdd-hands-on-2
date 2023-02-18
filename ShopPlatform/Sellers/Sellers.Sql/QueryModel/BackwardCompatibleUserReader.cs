using Microsoft.EntityFrameworkCore;

namespace Sellers.QueryModel;

public sealed class BackwardCompatibleUserReader: IUserReader
{
    private readonly Func<SellersDbContext> contextFactory;

    public BackwardCompatibleUserReader(Func<SellersDbContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public async Task<User?> FindUser(string username)
    {
        SqlUserReader userReader = new (this.contextFactory);
        return await userReader.FindUser(username);
    }
}