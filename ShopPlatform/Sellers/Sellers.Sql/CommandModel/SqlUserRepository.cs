using System.Data.Common;

namespace Sellers.CommandModel;

public sealed class SqlUserRepository : IUserRepository
{
    private readonly Func<SellersDbContext> contextFactory;

    public SqlUserRepository(Func<SellersDbContext> contextFactory) 
        => this.contextFactory = contextFactory;

    public async Task Add(User user)
    {
        using SellersDbContext context = this.contextFactory.Invoke();
        context.Users.Add(new UserEntity {Username = user.Username, Id = user.Id, PasswordHash = user.PasswordHash});
        await context.SaveChangesAsync();
    }
}