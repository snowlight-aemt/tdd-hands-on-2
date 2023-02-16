using Microsoft.EntityFrameworkCore;

namespace Sellers.QueryModel;

public class SqlUserReader: IUserReader
{
    private readonly Func<SellersDbContext> contextFactory;

    public SqlUserReader(Func<SellersDbContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    public async Task<User?> FindUser(string username)
    {
        SellersDbContext context = this.contextFactory.Invoke();
        UserEntity userEntity = await context.Users.AsNoTracking().Where(x => x.Username == username).SingleAsync();
        return new User(userEntity.Id, userEntity.Username, userEntity.PasswordHash);
    }
}