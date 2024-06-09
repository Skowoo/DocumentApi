using DocumentApi.Domain.Entities;
using DocumentApi.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using DocumentApi.Web;
using System.Net;

namespace DocumentApi.Web_IntegrationTests.DataFixtures
{
    public class DocumentApiDataFixture : IDisposable
    {
        public HttpClient Client { get; }

        public string AdminToken { get; }

        public string UserToken { get; }

        public string ExampleDocumentId { get; }

        public DocumentApiDataFixture()
        {
            var app = new DocumentApiFactory<Program>();
            var scope = app.Services.CreateScope();
            SeedDatabase(scope).Wait();
            Client = app.CreateClient();

            ExampleDocumentId = GetExampleDocumentId(scope);
            AdminToken = $"Bearer {GetAdminTokenAsync().Result}";
            UserToken = $"Bearer {GetUserTokenAsync().Result}";
        }

        public void Dispose() => Client.Dispose();

        static async Task SeedDatabase(IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetRequiredService<DocumentDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var seeder = new TestDataSeeder(context, userManager, roleManager);
            await seeder.Initialize();
        }

        async Task<string> GetAdminTokenAsync()
        {
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Identity/Login"),
                Method = HttpMethod.Post,
                Headers =
                    {
                        {HttpRequestHeader.ContentType.ToString(), "application/json"}
                    },
                Content = JsonContent.Create(new AppUser() { Login = TestDataSeeder.adminCredentials, Password = TestDataSeeder.adminCredentials }),
            };
            var result = Client.SendAsync(request).Result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsStringAsync();
        }

        async Task<string> GetUserTokenAsync()
        {
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Identity/Login"),
                Method = HttpMethod.Post,
                Headers =
                    {
                        {HttpRequestHeader.ContentType.ToString(), "application/json"}
                    },
                Content = JsonContent.Create(new AppUser() { Login = TestDataSeeder.userCredentials, Password = TestDataSeeder.userCredentials }),
            };
            var result = Client.SendAsync(request).Result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsStringAsync();
        }

        string GetExampleDocumentId(IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetRequiredService<DocumentDbContext>();
            var document = context.Documents.FirstOrDefault();

            if (document is null || document.Id == Guid.Empty)
                throw new Exception("Document initialization failed!");

            return document.Id.ToString();
        }
    }
}
