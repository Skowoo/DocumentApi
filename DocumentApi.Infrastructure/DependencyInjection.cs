using Microsoft.Extensions.DependencyInjection;
using DocumentApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using DocumentApi.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using DocumentApi.Infrastructure.Identity.Services;
using DocumentApi.Infrastructure.Identity.DependencyInjection;

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


            thisService.AddScoped<IUserService, UserService>();


            thisService.AddScoped<IDocumentDbContext>(provider 
                => provider.GetRequiredService<DocumentDbContext>());


            return thisService;
        }
    }
}
