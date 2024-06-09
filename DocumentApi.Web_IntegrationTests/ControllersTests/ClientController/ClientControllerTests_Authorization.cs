using DocumentApi.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using DocumentApi.Domain.Entities;
using DocumentApi.Application.Clients.Commands.CreateClient;
using DocumentApi.Application.Clients.Commands.UpdateClient;

namespace DocumentApi.Web_IntegrationTests.ControllersTests.ClientController
{
    public class ClientControllerTests_Authorization
    {
        [Fact]
        public async void GetAll_ShouldReturnAuthorizationError()
        {
            await using var app = new WebApplicationFactory<Program>();
            using var client = app.CreateClient();

            var response = await client.GetAsync("/api/Client/GetAll");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1978235653)]
        public async void GetById_ShouldReturnAuthorizationError(int id)
        {
            await using var app = new WebApplicationFactory<Program>();
            using var client = app.CreateClient();

            var response = await client.GetAsync($"/api/Client/GetById/{id}");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void Add_ShouldReturnAuthorizationError()
        {
            await using var app = new WebApplicationFactory<Program>();
            using var client = app.CreateClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Client/Add"),
                Method = HttpMethod.Post,
                Headers =
                    {                        
                        {HttpRequestHeader.ContentType.ToString(), "application/json"}
                    },
                Content = JsonContent.Create(new CreateClientCommand() { Name = "ValidName", Email = "valid@email.pl", TelephoneNumber = "1234567890"}),
            };

            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void Update_ShouldReturnAuthorizationError()
        {
            await using var app = new WebApplicationFactory<Program>();
            using var client = app.CreateClient();
            HttpRequestMessage request = new()
            {
                RequestUri = new Uri("https://localhost:7176/api/Client/Add"),
                Method = HttpMethod.Post,
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
            await using var app = new WebApplicationFactory<Program>();
            using var client = app.CreateClient();

            var response = await client.DeleteAsync($"/api/Client/Delete/{id}");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}