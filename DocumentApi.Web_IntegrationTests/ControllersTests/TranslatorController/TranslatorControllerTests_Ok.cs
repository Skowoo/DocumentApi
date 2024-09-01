using DocumentApi.Application.Translators.Commands.CreateTranslator;
using DocumentApi.Application.Translators.Commands.UpdateTranslator;
using DocumentApi.Domain.Entities;
using DocumentApi.Web_IntegrationTests.DataFixtures;
using Newtonsoft.Json;
using System.Net;

namespace DocumentApi.Web_IntegrationTests.ControllersTests.TranslatorController
{
    public class TranslatorControllerTests_Ok(DocumentApiDataFixture fixture) : IClassFixture<DocumentApiDataFixture>
    {
        readonly HttpClient client = fixture.Client;

        [Fact]
        public async Task GetAll_ShouldReturnOk()
        {
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Translator/GetAll"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                }
            };

            var response = await client.SendAsync(request);
            var returnedItem = JsonConvert.DeserializeObject<List<Translator>>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnedItem);
            Assert.Contains(returnedItem, x => x.Id == 1);
            Assert.Contains(returnedItem, x => x.Id == 3);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk()
        {
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Translator/GetById/1"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                }
            };

            var response = await client.SendAsync(request);
            var returnedItem = JsonConvert.DeserializeObject<Translator>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnedItem);
            Assert.Equal(1, returnedItem.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFoundWhenNoData()
        {
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Translator/GetById/100"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                }
            };

            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Add_ShouldReturnOkAndReturnNewId()
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
                Content = JsonContent.Create(new CreateTranslatorCommand() { Name = "ValidName" })
            };

            var response = await client.SendAsync(request);
            var returnedItem = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(4, returnedItem);


            // Check request
            HttpRequestMessage checkRequest = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Translator/GetById/4"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                }
            };

            var checkResponse = await client.SendAsync(checkRequest);
            var newItem = JsonConvert.DeserializeObject<Translator>(await checkResponse.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, checkResponse.StatusCode);
            Assert.NotNull(newItem);
            Assert.Equal(4, newItem.Id);
            Assert.Contains("Valid", newItem.Name);
        }

        [Fact]
        public async Task Update_ShouldReturnOk()
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
                Content = JsonContent.Create(new UpdateTranslatorCommand() { Id = 1, Name = "updated" })
            };

            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);


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
            Assert.Contains("updated", returnedItem.Name);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk()
        {
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Translator/Delete/2"),
                Method = HttpMethod.Delete,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                }
            };

            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            HttpRequestMessage checkRequest = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Translator/GetById/2"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                }
            };

            var checkResponse = await client.SendAsync(checkRequest);

            Assert.Equal(HttpStatusCode.NotFound, checkResponse.StatusCode);
        }
    }
}
