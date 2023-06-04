using DAL.DbContexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WebAPITests;
public partial class TestBase
{
    protected DbContextBase dbContext;
    protected HttpClient webApiClient;

    public TestBase()
    {
        var webApiFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(DbContextBase));
                    services.AddDbContext<DbContextBase>(options => {
                        options.UseInMemoryDatabase("TestDb");
                    });
                });
            });

        dbContext = new(null);
        webApiClient = webApiFactory.CreateClient();
    }
}
