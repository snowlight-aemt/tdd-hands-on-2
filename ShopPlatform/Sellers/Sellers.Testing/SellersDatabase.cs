using Microsoft.EntityFrameworkCore;

namespace Sellers;

public static class SellersDatabase
{
    public const string DefaultConnectionString = "Server=127.0.0.1;Port=5432;Database=Sellers_UnitTests;User Id=postgres;Password=mysecretpassword;";

    public static SellersDbContext GetContext(string connectionString = DefaultConnectionString)
    {
        DbContextOptionsBuilder<SellersDbContext> builder = new();
        return new SellersDbContext(builder.UseNpgsql(connectionString).Options);
    }
}