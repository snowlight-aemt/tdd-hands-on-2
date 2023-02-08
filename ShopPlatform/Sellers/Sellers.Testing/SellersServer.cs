using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Sellers;

public sealed class SellersServer : TestServer
{
    private const string DefaultConnectionString = "Server=127.0.0.1;Port=5432;Database=Sellers_UnitTests;User Id=postgres;Password=mysecretpassword;";
        
    public SellersServer(
        IServiceProvider services, 
        IOptions<TestServerOptions> optionsAccessor) 
        : base(services, optionsAccessor)
    {
        
    }

    public static SellersServer Create(string connectionString = DefaultConnectionString)
    {
        SellersServer server =  (SellersServer) new Factory(connectionString).Server;
        lock (typeof(SellersDbContext))
        {
            using IServiceScope scope = server.Services.CreateScope();
            SellersDbContext context = scope.ServiceProvider.GetRequiredService<SellersDbContext>();
            context.Database.Migrate();
        }
        return server;
    }

    private sealed class Factory : WebApplicationFactory<Program>
    {
        private readonly string connectionString;
        
        public Factory(string connectionString) => this.connectionString = connectionString;

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // IServer 서비스 타입을 OrdersServer 로 구현한다고 선언함.  
                services.AddSingleton<IServer, SellersServer>();
            });

            builder.ConfigureAppConfiguration(config =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ConnectionStrings:DefaultConnection"] = this.connectionString,
                });
            });
            
            return base.CreateHost(builder);
        }
    }
}