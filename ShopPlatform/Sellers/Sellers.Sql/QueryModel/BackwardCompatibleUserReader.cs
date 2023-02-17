namespace Sellers.QueryModel;

public sealed class BackwardCompatibleUserReader: IUserReader
{
    private readonly Func<SellersDbContext> contextFactory;

    public BackwardCompatibleUserReader(Func<SellersDbContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public Task<User?> FindUser(string username)
    {
        throw new NotImplementedException();
    }
}