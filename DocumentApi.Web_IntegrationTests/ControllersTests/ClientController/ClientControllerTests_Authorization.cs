using DocumentApi.Application.Clients.Commands.CreateClient;
using DocumentApi.Application.Clients.Commands.UpdateClient;
using DocumentApi.Web_IntegrationTests.DataFixtures;
using System.Net;

namespace DocumentApi.Web_IntegrationTests.ControllersTests.ClientController
{
    [Collection("DocumentApiCollection")]
    public class ClientControllerTests_Authorization(DocumentApiDataFixture fixture) : IClassFixture<DocumentApiDataFixture>
    {
        readonly HttpClient client = fixture.Client;

        [Fact]
        public async void GetAll_ShouldReturnAuthorizationError()
        {
            var response = await client.GetAsync("/api/Client/GetAll");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1978235653)]
        public async void GetById_ShouldReturnAuthorizationError(int id)
        {
            var response = await client.GetAsync($"/api/Client/GetById/{id}");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void Add_ShouldReturnAuthorizationError()
        {
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Client/Add"),
                Method = HttpMethod.Post,
                Headers =
                    {
                        {HttpRequestHeader.ContentType.ToString(), "application/json"}
                    },
                Content = JsonContent.Create(new CreateClientCommand() { Name = "ValidName", Email = "valid@email.pl", TelephoneNumber = "1234567890" }),
            };

            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void Update_ShouldReturnAuthorizationError()
        {
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Client/Update"),
                Method = HttpMethod.Put,
                Headers =
                    {
                        {HttpRequestHeader.ContentType.ToString(), "application/json"}
                    },
                Content = JsonContent.Create(new UpdateClientCommand() { Id = 1, Name = "ValidName", Email = "valid@email.pl", TelephoneNumber = "1234567890" }),
            };

            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1978235653)]
        public async void Delete_ShouldReturnAuthorizationError(int id)
        {
            var response = await client.DeleteAsync($"/api/Client/Delete/{id}");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}