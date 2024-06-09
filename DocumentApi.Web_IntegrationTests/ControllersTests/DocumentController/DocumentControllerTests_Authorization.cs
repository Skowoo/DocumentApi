using System.Net;
using DocumentApi.Web_IntegrationTests.DataFixtures;
using DocumentApi.Application.Documents.Commands.CreateDocument;
using DocumentApi.Application.Documents.Commands.UpdateDocument;

namespace DocumentApi.Web_IntegrationTests.ControllersTests.DocumentController
{
    [Collection("DocumentApiCollection")]
    public class DocumentControllerTests_Authorization(DocumentApiDataFixture fixture) : IClassFixture<DocumentApiDataFixture>
    {
        readonly HttpClient client = fixture.Client;

        [Fact]
        public async void GetAll_ShouldReturnAuthorizationError()
        {
            var response = await client.GetAsync("/api/Document/GetAll");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void GetById_ShouldReturnAuthorizationError()
        {
            var response = await client.GetAsync($"/api/Document/GetById/{fixture.ExampleDocumentId}");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void Add_ShouldReturnAuthorizationError()
        {
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Document/Add"),
                Method = HttpMethod.Post,
                Headers =
                    {
                        {HttpRequestHeader.ContentType.ToString(), "application/json"}
                    },
                Content = JsonContent.Create(new CreateDocumentCommand() { Title = "ValidTitle", SignsSize = 1000, Deadline = DateTime.Now.AddDays(2), ClientId = 1, TranslatorId = 1 }),
            };

            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void Update_ShouldReturnAuthorizationError()
        {
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Translator/Update"),
                Method = HttpMethod.Put,
                Headers =
                    {
                        {HttpRequestHeader.ContentType.ToString(), "application/json"}
                    },
                Content = JsonContent.Create(new UpdateDocumentCommand() { Id = Guid.NewGuid(), Title = "ValidTitle", SignsSize = 1000, CreatedAt = DateTime.Now, Deadline = DateTime.Now.AddDays(2), ClientId = 1, TranslatorId = 1 }),
            };

            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1978235653)]
        public async void Delete_ShouldReturnAuthorizationError(int id)
        {
            var response = await client.DeleteAsync($"/api/Document/Delete/{id}");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
