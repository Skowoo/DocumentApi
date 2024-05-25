using Microsoft.Extensions.DependencyInjection;
using DocumentApi.Infrastructure.Data.Services;
using DocumentApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DocumentApi.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DocumentApi.Infrastructure
{
    public static class DependencyInjection
    {
        // Extension method used to register services from Infrastructure layer into Web application DI container
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection thisService, IConfiguration configuration)
        {
            // Register InMemory database
            thisService.AddDbContext<DocumentDbContext>(options => options.UseInMemoryDatabase("MemoDb"));

            // Register and configure Identity options
            thisService.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DocumentDbContext>();

            // Configure Identiy module
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

            // Register Authentication as Jwt token
            thisService.AddAuthorization();
            thisService.AddAuthentication(options => // Parametrize authentication
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => // Parametrize token
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

            // Register services
            thisService.AddScoped<IUserService, UserService>();

            // Register DbContext Interface
            thisService.AddScoped<IDocumentDbContext>(provider => provider.GetRequiredService<DocumentDbContext>());

            return thisService;
        }
    }
}
