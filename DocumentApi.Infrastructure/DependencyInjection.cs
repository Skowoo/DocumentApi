using DocumentApi.Infrastructure.Data.MemoryService;
using DocumentApi.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentApi.Infrastructure
{
    public static class DependencyInjection
    {
        // Extension method used to register services from Infrastructure layer into Web application DI container
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection thisService)
        {
            thisService.AddSingleton<IDocumentService, DocumentMemoryService>();

            return thisService;
        }
    }
}
