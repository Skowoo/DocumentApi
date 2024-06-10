using DocumentApi.Application.Translators.Commands.CreateTranslator;
using DocumentApi.Application.Translators.Commands.UpdateTranslator;
using DocumentApi.Domain.Entities;
using DocumentApi.Web_IntegrationTests.DataFixtures;
using Newtonsoft.Json;
using System.Net;

namespace DocumentApi.Web_IntegrationTests.ControllersTests.TranslatorController
{
    public class TranslatorControllerTests_Validation(DocumentApiDataFixture fixture) : IClassFixture<DocumentApiDataFixture>
    {
        readonly HttpClient client = fixture.Client;

        [Fact]
        public async Task Add_ShouldReturnBadRequest()
        {
            // Main request
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Translator/Add"),
                Method = HttpMethod.Post,
                Headers =
                {
                    {HttpRequestHeader.ContentType.ToString(), "application/json"},
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                },
                Content = JsonContent.Create(new CreateTranslatorCommand() { Name = "" })
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
                RequestUri = new Uri("https://localhost:7176/api/Translator/Update"),
                Method = HttpMethod.Put,
                Headers =
                {
                    {HttpRequestHeader.ContentType.ToString(), "application/json"},
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                },
                Content = JsonContent.Create(new UpdateTranslatorCommand() { Id = 1, Name = "" })
            };

            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            // Check request
            HttpRequestMessage checkRequest = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Translator/GetById/1"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                }
            };

            var checkResponse = await client.SendAsync(checkRequest);
            var returnedItem = JsonConvert.DeserializeObject<Translator>(await checkResponse.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, checkResponse.StatusCode);
            Assert.NotNull(returnedItem);
            Assert.Equal(1, returnedItem.Id);
            Assert.DoesNotContain("updated", returnedItem.Name);
        }
    }
}
