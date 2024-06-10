using DocumentApi.Application.Documents.Commands.CreateDocument;
using DocumentApi.Application.Documents.Commands.UpdateDocument;
using DocumentApi.Domain.Entities;
using DocumentApi.Web_IntegrationTests.DataFixtures;
using Newtonsoft.Json;
using System.Net;

namespace DocumentApi.Web_IntegrationTests.ControllersTests.DocumentController
{
    [Collection("DocumentApiCollection")]
    public class DocumentControllerTests_Validation(DocumentApiDataFixture fixture) : IClassFixture<DocumentApiDataFixture>
    {
        readonly HttpClient client = fixture.Client;

        [Fact]
        public async Task AddWithWrongSignsSize_ShouldReturnBadRequest()
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
                Content = JsonContent.Create(new CreateDocumentCommand() { Title = "ValidTitle", SignsSize = 0, Deadline = deadline, ClientId = 1, TranslatorId = 1 })
            };

            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequestAndNotUpdateItem()
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
                Content = JsonContent.Create(new UpdateDocumentCommand() { Id = Guid.Parse(fixture.ExampleDocumentId), Title = "Updated", SignsSize = 0, CreatedAt = creationDate, Deadline = deadline, ClientId = 2, TranslatorId = 2 })
            };

            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

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
            Assert.DoesNotContain("Updated", newItem.Title);
        }
    }
}
