using DocumentApi.Application.Common.Behaviours;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DocumentApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection appServices)
        {
            // Register FluentValidation with command and queries taken from assembly
            appServices.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Register MediatR
            appServices.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); // Register command and queries (taken from Assembly)
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>)); // Register pipeline to activate validation
            });

            return appServices;
        }
    }
}
