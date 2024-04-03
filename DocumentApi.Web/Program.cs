using DocumentApi.Infrastructure;

namespace DocumentApi.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(); // Add controllers in build - required for mapping

            builder.Services.AddInfrastructureServices();

            var app = builder.Build();
                        
            app.MapControllers(); // Register controllers with endpoints

            app.Run();
        }
    }
}
