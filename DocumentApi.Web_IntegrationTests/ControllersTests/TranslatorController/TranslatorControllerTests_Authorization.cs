using DocumentApi.Application.Translators.Commands.CreateTranslator;
using DocumentApi.Application.Translators.Commands.UpdateTranslator;
using DocumentApi.Web_IntegrationTests.DataFixtures;
using System.Net;

namespace DocumentApi.Web_IntegrationTests.ControllersTests.TranslatorController
{
    [Collection("DocumentApiCollection")]
    public class TranslatorControllerTests_Authorization(DocumentApiDataFixture fixture) : IClassFixture<DocumentApiDataFixture>
    {
        readonly HttpClient client = fixture.Client;

        [Fact]
        public async void GetAll_ShouldReturnAuthorizationError()
        {
            var response = await client.GetAsync("/api/Translator/GetAll");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1978235653)]
        public async void GetById_ShouldReturnAuthorizationError(int id)
        {
            var response = await client.GetAsync($"/api/Translator/GetById/{id}");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void Add_ShouldReturnAuthorizationError()
        {
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Translator/Add"),
                Method = HttpMethod.Post,
                Headers =
                    {
                        {HttpRequestHeader.ContentType.ToString(), "application/json"}
                    },
                Content = JsonContent.Create(new CreateTranslatorCommand() { Name = "ValidName" }),
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
                Content = JsonContent.Create(new UpdateTranslatorCommand() { Id = 1, Name = "ValidName" }),
            };

            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1978235653)]
        public async void Delete_ShouldReturnAuthorizationError(int id)
        {
            var response = await client.DeleteAsync($"/api/Translator/Delete/{id}");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
