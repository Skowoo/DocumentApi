using Microsoft.Extensions.DependencyInjection;
using DocumentApi.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace DocumentApi.Infrastructure.Identity.DependencyInjection
{
    internal static class RegisterAndConfigureIdentityModule
    {
        public static IServiceCollection AddIdentityModule(this IServiceCollection services)
        {
            services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DocumentDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = false;
            });

            return services;
        }
    }
}
