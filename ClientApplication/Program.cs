using ClientApplication.Classes;
using ClientApplication.Config;
using ClientApplication.Interfaces;
using ClientApplication.Services;

namespace ClientApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var apiConfigSection = builder.Configuration.GetSection("DocumentApiConfig");
            builder.Services.Configure<DocumentApiConfig>(apiConfigSection);

            builder.Services.AddSingleton<CurrentUser>();

            builder.Services.AddTransient(typeof(IRestService<>), typeof(RestService<>));
            builder.Services.AddTransient<IIdentityRestService, IdentityRestService>();
            builder.Services.AddTransient<SelectListHelper>();

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
