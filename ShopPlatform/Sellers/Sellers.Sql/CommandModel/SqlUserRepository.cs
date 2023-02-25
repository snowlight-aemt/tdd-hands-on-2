using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Sellers.CommandModel;

public sealed class SqlUserRepository : IUserRepository
{
    private readonly Func<SellersDbContext> contextFactory;

    public SqlUserRepository(Func<SellersDbContext> contextFactory) 
        => this.contextFactory = contextFactory;

    public async Task Add(User user)
    {
        using SellersDbContext context = this.contextFactory.Invoke();
        context.Users.Add(new UserEntity
        {
            Username = user.Username,
            Id = user.Id,
            PasswordHash = user.PasswordHash,
            // Roles = user.Roles,
        });
        await context.SaveChangesAsync();
    }

    public async Task<bool> TryUpdate(Guid id, Func<User, User> reviser)
    {
        using SellersDbContext context = this.contextFactory.Invoke();
        UserEntity? user = await context.Users.SingleOrDefaultAsync(x => x.Id == id);
    
        return user != null;
    }
}