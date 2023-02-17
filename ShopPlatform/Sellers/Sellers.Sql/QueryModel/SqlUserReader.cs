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
        IQueryable<UserEntity> query = context.Users.AsNoTracking().Where(x => x.Username == username);
        return await query.SingleOrDefaultAsync() switch
        {
            UserEntity user => new User(user.Id, user.Username, user.PasswordHash),
            null => null,
        };
    }
}