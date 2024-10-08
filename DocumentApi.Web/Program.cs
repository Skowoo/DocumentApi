using DocumentApi.Application;
using DocumentApi.Infrastructure;
using DocumentApi.Infrastructure.Data;
using DocumentApi.Web.Middleware;
using Microsoft.OpenApi.Models;

namespace DocumentApi.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(); // Add controllers in build - required for mapping

            // Register layers
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices();

            // Add Swagger window for accepting Jwt Token - need to download NuGet Swashbuckle.AspNetCore
            builder.Services.AddSwaggerGen(section =>
            {
                section.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Type in 'Bearer' followed by space and token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });

                section.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            // Use Authentication and Autorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Register custom middleware to handle validation exceptions
            app.UseMiddleware<ValidationExceptionHandlingMiddleware>();

            // Use Swagger in development mode only:
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.Map("/", () => Results.Redirect("/swagger"));
                await app.SeedDatabaseAsync();
            }

            app.MapControllers(); // Register controllers with endpoints

            app.Run();
        }
    }
}
