using ClientApplication.Classes;
using ClientApplication.Config;
using ClientApplication.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace ClientApplication.Services
{
    public class ApiClientService : IApiClientService
    {
        RestClient Client;

        public ApiClientService(CurrentUser user, IOptions<DocumentApiConfig> config)
        {
            var options = new RestClientOptions(config.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            Client = new RestClient(options);
        }

        public async Task<ApiResponse<Client>> CreateAsync(Client client)
        {
            var request = new RestRequest("/Client/Add", Method.Post);
            var payload = new
            {
                client.Name,
                client.Email,
                client.TelephoneNumber
            };
            request.AddBody(payload);

            var response = await Client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {
                client.Id = int.Parse(response.Content!);
                return new ApiResponse<Client>(true, client, null);
            }
            else if (response.Content is not null)
            {
                var errors = JObject.Parse(response.Content)["Errors"];
                List<(string Error, string Message)> errorsParsed = [];
                if (errors is not null)
                    foreach (var error in errors)
                    {
                        string propertyName = $"{nameof(Client)}.{error["propertyName"]}";
                        string errorMessage = error["errorMessage"]!.ToString();
                        errorsParsed.Add((propertyName, errorMessage));
                    }

                return new ApiResponse<Client>(false, null, errorsParsed);
            }
            return new ApiResponse<Client>(false, null, [("General", "Request failed without Validation errors!")]);
        }

        public async Task<ApiResponse<Client>> DeleteAsync(int id)
        {
            var request = new RestRequest($"/Client/Delete/{id}", Method.Delete);

            var response = await Client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
                return new ApiResponse<Client>(true, null, null);

            return new ApiResponse<Client>(false, null, [("General", "Request failed!")]);
        }

        public async Task<ApiResponse<List<Client>>> GetAllAsync()
        {
            var request = new RestRequest("/Client/GetAll", Method.Get);
            var response = await Client.ExecuteAsync(request);
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                var value = JsonConvert.DeserializeObject<List<Client>>(response.Content!)!;
                return new ApiResponse<List<Client>>(true, value, null);
            }
            else
            {
                return new ApiResponse<List<Client>>(true, null, [("General", "Request Failed!")]);
            }
        }

        public async Task<ApiResponse<Client>> GetByIdAsync(int id)
        {
            var request = new RestRequest($"Client/GetById/{id}");
            var response = await Client.ExecuteAsync(request, Method.Get);
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                var downloadedEntity = JsonConvert.DeserializeObject<Client>(response.Content)!;
                if (downloadedEntity is not null)
                    return new ApiResponse<Client>(true, downloadedEntity, null);
            }
            return new ApiResponse<Client>(false, null, [("General", "Translator not found!")]);
        }

        public async Task<ApiResponse<Client>> UpdateAsync(Client translator)
        {
            var request = new RestRequest("/Client/Update", Method.Put);

            var payload = new
            {
                translator.Id,
                translator.Name,
                translator.Email,
                translator.TelephoneNumber
            };
            request.AddBody(payload);

            var response = await Client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<Client>(true, translator, null);
            }
            else if (response.Content is not null)
            {
                var errors = JObject.Parse(response.Content)["Errors"];
                List<(string Error, string Message)> errorsParsed = [];
                if (errors is not null)
                    foreach (var error in errors)
                    {
                        string propertyName = $"{nameof(Client)}.{error["propertyName"]}";
                        string errorMessage = error["errorMessage"]!.ToString();
                        errorsParsed.Add((propertyName, errorMessage));
                    }

                return new ApiResponse<Client>(false, null, errorsParsed);
            }
            return new ApiResponse<Client>(false, null, [("General", "Request failed without Validation errors!")]);
        }
    }
}
