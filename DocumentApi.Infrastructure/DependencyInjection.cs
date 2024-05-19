using Microsoft.Extensions.DependencyInjection;
using DocumentApi.Application.Interfaces;
using DocumentApi.Infrastructure.Data.Services;
using DocumentApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DocumentApi.Infrastructure
{
    public static class DependencyInjection
    {
        // Extension method used to register services from Infrastructure layer into Web application DI container
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection thisService)
        {
            // Register InMemory database
            thisService.AddDbContext<DocumentDbContext>(options => options.UseInMemoryDatabase("MemoDb"));

            // Register and configure Identity options
            thisService.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DocumentDbContext>();

            thisService.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                options.User.RequireUniqueEmail = false;
            });

            // Register services
            thisService.AddScoped<IDocumentService, DocumentService>();
            thisService.AddScoped<ITranslatorService, TranslatorService>();
            thisService.AddScoped<IUserService, UserService>();

            // Register DbContext interface
            thisService.AddScoped<IDocumentDbContext>(provider => provider.GetRequiredService<DocumentDbContext>());

            return thisService;
        }
    }
}
