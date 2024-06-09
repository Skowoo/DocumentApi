using DocumentApi.Web;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DocumentApi.Web_IntegrationTests.DataFixtures
{
    public class DocumentApiDataFixture : IDisposable
    {
        public HttpClient Client { get; }

        public DocumentApiDataFixture()
        {
            var app = new WebApplicationFactory<Program>();
            Client = app.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
