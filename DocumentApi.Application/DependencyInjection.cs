using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DocumentApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection appServices)
        {
            appServices.AddMediatR(cfg => { 
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); 
            });

            return appServices;
        }
    }
}
