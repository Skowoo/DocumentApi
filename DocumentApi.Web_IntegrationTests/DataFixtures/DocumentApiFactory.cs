using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using DocumentApi.Infrastructure.Data;
using DocumentApi.Application.Common.Interfaces;
using Moq;

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

                var grpcTimeService = services.SingleOrDefault(d => d.ServiceType == typeof(ITimeProvider));
                services.Remove(grpcTimeService!);

                var timeServiceMock = new Mock<ITimeProvider>();
                timeServiceMock.Setup(x => x.GetCurrentTimeAsync()).ReturnsAsync(DateTime.Now);
                services.AddSingleton(timeServiceMock.Object);

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
