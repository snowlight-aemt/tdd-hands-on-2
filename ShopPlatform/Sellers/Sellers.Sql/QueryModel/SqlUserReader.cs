using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Sellers.QueryModel;

public class SqlUserReader: IUserReader
{
    private readonly Func<SellersDbContext> contextFactory;

    public SqlUserReader(Func<SellersDbContext> contextFactory) 
        => this.contextFactory = contextFactory;

    public Task<User?> FindUser(string username) => FindUser(x => x.Username == username);

    public Task<User?> FindUser(Guid id) => FindUser(x => x.Id == id);

    private async Task<User?> FindUser(Expression<Func<UserEntity, bool>> predicate)
    {
        SellersDbContext context = this.contextFactory.Invoke();
        IQueryable<UserEntity> query = context.Users.AsNoTracking().Where(predicate);
        return await query.SingleOrDefaultAsync() switch
        {
            UserEntity user => new User(user.Id, user.Username, user.PasswordHash),
            null => null,
        };
    }
}