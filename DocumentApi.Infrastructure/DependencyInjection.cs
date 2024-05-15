using Microsoft.Extensions.DependencyInjection;
using DocumentApi.Application.Interfaces;
using DocumentApi.Infrastructure.Data.Services;
using DocumentApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentApi.Infrastructure
{
    public static class DependencyInjection
    {
        // Extension method used to register services from Infrastructure layer into Web application DI container
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection thisService)
        {
            // Register InMemory database
            thisService.AddDbContext<DocumentDbContext>(options => options.UseInMemoryDatabase("MemoDb"));

            // Register services
            thisService.AddScoped<IDocumentService, DocumentService>();
            thisService.AddScoped<IClientService, ClientService>();
            thisService.AddScoped<ITranslatorService, TranslatorService>();
            thisService.AddScoped<IUserService, UserService>();

            return thisService;
        }
    }
}
