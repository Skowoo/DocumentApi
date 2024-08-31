using DocumentApi.Application.Common.Interfaces;
using DocumentApi.Infrastructure.Data;
using DocumentApi.Infrastructure.Identity.DependencyInjection;
using DocumentApi.Infrastructure.Identity.Services;
using DocumentApi.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentApi.Infrastructure
{
    public static class DependencyInjection
    {
        // Extension method used to register services from Infrastructure layer into Web application DI container
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection thisService, IConfiguration configuration)
        {
            thisService.AddDbContext<DocumentDbContext>(options
                => options.UseInMemoryDatabase("MemoDb"));

            thisService.AddIdentityModule();
            thisService.AddJwtAuthentication(configuration);

            thisService.AddScoped<DbInitializer>();
            thisService.AddScoped<IUserService, UserService>();
            thisService.AddScoped<ITimeProvider, BasicTimeProvider>();

            thisService.AddScoped<IDocumentDbContext>(provider
                => provider.GetRequiredService<DocumentDbContext>());


            return thisService;
        }
    }
}
