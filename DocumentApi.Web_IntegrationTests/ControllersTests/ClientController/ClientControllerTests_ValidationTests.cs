using DocumentApi.Application.Clients.Commands.CreateClient;
using DocumentApi.Application.Clients.Commands.UpdateClient;
using DocumentApi.Domain.Entities;
using DocumentApi.Web_IntegrationTests.DataFixtures;
using Newtonsoft.Json;
using System.Net;

namespace DocumentApi.Web_IntegrationTests.ControllersTests.ClientController
{
    [Collection("DocumentApiCollection")]
    public class ClientControllerTests_ValidationTests(DocumentApiDataFixture fixture) : IClassFixture<DocumentApiDataFixture>
    {
        readonly HttpClient client = fixture.Client;

        [Fact]
        public async Task Add_ShouldReturnBadRequest()
        {
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Client/Add"),
                Method = HttpMethod.Post,
                Headers =
                {
                    {HttpRequestHeader.ContentType.ToString(), "application/json"},
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                },
                Content = JsonContent.Create(new CreateClientCommand() { Name = "ValidName", Email = "email.com", TelephoneNumber = "ValidNumber" })
            };

            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequestAndNotUpdateItem()
        {
            // Main request
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Client/Update"),
                Method = HttpMethod.Put,
                Headers =
                {
                    {HttpRequestHeader.ContentType.ToString(), "application/json"},
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                },
                Content = JsonContent.Create(new UpdateClientCommand() { Id = 1, Name = "updated", Email = "email.com", TelephoneNumber = "updated890" })
            };

            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            // Check request
            HttpRequestMessage checkRequest = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Client/GetById/1"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                }
            };

            var checkResponse = await client.SendAsync(checkRequest);
            var returnedItem = JsonConvert.DeserializeObject<Client>(await checkResponse.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, checkResponse.StatusCode);
            Assert.NotNull(returnedItem);
            Assert.Equal(1, returnedItem.Id);
            Assert.DoesNotContain("updated", returnedItem.Name);
            Assert.DoesNotContain("updated", returnedItem.Email);
            Assert.DoesNotContain("updated", returnedItem.TelephoneNumber);
        }
    }
}