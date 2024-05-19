using DocumentApi.Infrastructure;
using DocumentApi.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace DocumentApi.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(); // Add controllers in build - required for mapping

            // Add Swagger - need to download NuGet Swashbuckle.AspNetCore
            builder.Services.AddSwaggerGen();

            builder.Services.AddInfrastructureServices();
            builder.Services.AddApplicationServices();

            // Register Authentication as Jwt token
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(options => // Parametrize authentication
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => // Parametrize token
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

            // Add Swagger window for accepting Jwt Token
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

            // Use Swagger in development mode only:
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.Map("/", () => Results.Redirect("/swagger"));
            }

            app.MapControllers(); // Register controllers with endpoints

            app.Run();
        }
    }
}
