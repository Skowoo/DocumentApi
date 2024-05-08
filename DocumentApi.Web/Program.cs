using DocumentApi.Infrastructure;

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



            var app = builder.Build();

            // Use Swagger in development mode only:
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers(); // Register controllers with endpoints

            app.Run();
        }
    }
}
