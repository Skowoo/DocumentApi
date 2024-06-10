#pragma warning disable IDE1006 // Naming Styles - naming convention broken in order to ensure correct test order

using DocumentApi.Application.Documents.Commands.CreateDocument;
using DocumentApi.Application.Documents.Commands.UpdateDocument;
using DocumentApi.Domain.Entities;
using DocumentApi.Web_IntegrationTests.DataFixtures;
using Newtonsoft.Json;
using System.Net;

namespace DocumentApi.Web_IntegrationTests.ControllersTests.DocumentController
{
    [Collection("DocumentApiCollection")]
    public class DocumentControllerTests_Ok(DocumentApiDataFixture fixture) : IClassFixture<DocumentApiDataFixture>
    {
        readonly HttpClient client = fixture.Client;

        [Fact]
        public async Task GetAll_ShouldReturnOk()
        {
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Document/GetAll"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                }
            };

            var response = await client.SendAsync(request);
            var returnedItem = JsonConvert.DeserializeObject<List<Document>>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnedItem);
            Assert.True(returnedItem.Count > 9);
            Assert.Contains(returnedItem, x => x.Id.ToString() == fixture.ExampleDocumentId);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk()
        {
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri($"https://localhost:7176/api/Document/GetById/{fixture.ExampleDocumentId}"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                }
            };

            var response = await client.SendAsync(request);
            var returnedItem = JsonConvert.DeserializeObject<Document>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnedItem);
            Assert.Equal(fixture.ExampleDocumentId, returnedItem.Id.ToString());
        }

        [Fact]
        public async Task Add_ShouldReturnOkAndReturnNewId()
        {
            // Main request
            DateTime deadline = DateTime.Now.AddDays(2);
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Document/Add"),
                Method = HttpMethod.Post,
                Headers =
                {
                    {HttpRequestHeader.ContentType.ToString(), "application/json"},
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                },
                Content = JsonContent.Create(new CreateDocumentCommand() { Title = "ValidTitle", SignsSize = 1000, Deadline = deadline, ClientId = 1, TranslatorId = 1 })
            };

            var response = await client.SendAsync(request);
            var returnedItem = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(Guid.TryParse(returnedItem, out Guid _));

            // Check request
            HttpRequestMessage checkRequest = new()
            {
                RequestUri = new Uri($"https://localhost:7176/api/Document/GetById/{returnedItem}"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                }
            };

            var checkResponse = await client.SendAsync(checkRequest);
            var newItem = JsonConvert.DeserializeObject<Document>(await checkResponse.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, checkResponse.StatusCode);
            Assert.NotNull(newItem);
            Assert.Equal(returnedItem, newItem.Id.ToString());
            Assert.Contains("Valid", newItem.Title);
            Assert.Equal(1000, newItem.SignsSize);
            Assert.Equal(deadline, newItem.Deadline);
            Assert.Equal(1, newItem.ClientId);
            Assert.Equal(1, newItem.TranslatorId);
        }

        [Fact]
        public async Task Update_ShouldReturnOk()
        {
            // Main request
            var deadline = DateTime.Now.AddDays(5);
            var creationDate = DateTime.Now.AddDays(1);
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Document/Update"),
                Method = HttpMethod.Put,
                Headers =
                {
                    {HttpRequestHeader.ContentType.ToString(), "application/json"},
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                },
                Content = JsonContent.Create(new UpdateDocumentCommand() { Id = Guid.Parse(fixture.ExampleDocumentId), Title = "Updated", SignsSize = 500, CreatedAt = creationDate, Deadline = deadline, ClientId = 2, TranslatorId = 2 })
            };

            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);


            // Check request
            HttpRequestMessage checkRequest = new()
            {
                RequestUri = new Uri($"https://localhost:7176/api/Document/GetById/{fixture.ExampleDocumentId}"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                }
            };

            var checkResponse = await client.SendAsync(checkRequest);
            var newItem = JsonConvert.DeserializeObject<Document>(await checkResponse.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, checkResponse.StatusCode);
            Assert.NotNull(newItem);
            Assert.Equal(fixture.ExampleDocumentId, newItem.Id.ToString());
            Assert.Contains("Updated", newItem.Title);
            Assert.Equal(500, newItem.SignsSize);
            Assert.Equal(creationDate, newItem.CreatedAt);
            Assert.Equal(deadline, newItem.Deadline);
            Assert.Equal(2, newItem.ClientId);
            Assert.Equal(2, newItem.TranslatorId);
        }

        [Fact]
        public async Task xDelete_ShouldReturnOk()
        {
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri($"https://localhost:7176/api/Document/Delete/{fixture.ExampleDocumentId}"),
                Method = HttpMethod.Delete,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                }
            };

            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            HttpRequestMessage checkRequest = new()
            {
                RequestUri = new Uri($"https://localhost:7176/api/Document/GetById/{fixture.ExampleDocumentId}"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), fixture.AdminToken}
                }
            };

            var checkResponse = await client.SendAsync(checkRequest);

            Assert.Equal(HttpStatusCode.NoContent, checkResponse.StatusCode);
        }
    }
}
