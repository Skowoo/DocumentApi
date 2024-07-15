using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using DocumentApi.Infrastructure.Data;

namespace DocumentApi.Web_IntegrationTests.DataFixtures
{
    public class DocumentApiFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var originalContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DocumentDbContext>));
                services.Remove(originalContext!);

                var dbConnection = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
                services.Remove(dbConnection!);

                services
                    .AddEntityFrameworkInMemoryDatabase()
                    .AddDbContext<DocumentDbContext>((container, options) =>
                    {
                        options.UseInMemoryDatabase("TestDatabase").UseInternalServiceProvider(container);
                    });
            });
            builder.UseEnvironment("Testing");
        }
    }
}
