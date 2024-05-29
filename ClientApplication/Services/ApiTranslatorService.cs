using ClientApplication.Classes;
using ClientApplication.Config;
using ClientApplication.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.Extensions.Options;
using RestSharp.Authenticators;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ClientApplication.Services
{
    public class ApiTranslatorService : IApiTranslatorService
    {
        RestClient Client { get; set; }

        public ApiTranslatorService(CurrentUser user, IOptions<DocumentApiConfig> config)
        {
            var options = new RestClientOptions(config.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            Client = new RestClient(options);
        }

        public async Task<ApiResponse<Translator>> Create(Translator translator)
        {
            var request = new RestRequest("/Translator/Add", Method.Post);
            var payload = new
            {
                translator.Name
            };
            request.AddBody(payload);
            var response = await Client.ExecuteAsync(request);
            if (response.IsSuccessStatusCode)
            {
                translator.Id = int.Parse(response.Content!);
                return new ApiResponse<Translator>(true, translator, null);
            }
            else if (response.Content is not null)
            {
                var errors = JObject.Parse(response.Content)["Errors"];
                List<(string Error, string Message)> errorsParsed = [];
                if (errors is not null)
                    foreach (var error in errors)
                    {                        
                        string propertyName = $"{nameof(Translator)}.{error["propertyName"]}";
                        string errorMessage = error["errorMessage"]!.ToString();
                        errorsParsed.Add((propertyName, errorMessage));
                    }

                return new ApiResponse<Translator>(false, null, errorsParsed);
            }
            return new ApiResponse<Translator>(false, null, [("General", "Request failed without Validation errors!")]);
        }

        public async Task<ApiResponse<Translator>> Delete(int id)
        {
            var request = new RestRequest($"/Translator/Delete/{id}", Method.Delete);
            var response = await Client.ExecuteAsync(request, Method.Delete);
            if (response.IsSuccessStatusCode)
                return new ApiResponse<Translator>(true, null, null);

            return new ApiResponse<Translator>(false, null, null);
        }

        public async Task<ApiResponse<List<Translator>>> GetAll()
        {
            var request = new RestRequest("/Translator/GetAll", Method.Get);
            var response = await Client.ExecuteAsync(request);
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                var value = JsonConvert.DeserializeObject<List<Translator>>(response.Content!)!;
                return new ApiResponse<List<Translator>>(true, value, null);
            }
            else
            {
                return new ApiResponse<List<Translator>>(true, null, [("General", "Request Failed!")]);
            }
        }

        public async Task<ApiResponse<Translator>> GetById(int id)
        {
            var request = new RestRequest($"Translator/GetById/{id}");
            var response = await Client.ExecuteAsync(request, Method.Get);
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                var downloadedEntity = JsonConvert.DeserializeObject<Translator>(response.Content)!;
                if (downloadedEntity is not null)
                    return new ApiResponse<Translator>(true, downloadedEntity, null);
            }
            return new ApiResponse<Translator>(false, null, [("General", "Translator not found!")]);
        }

        public async Task<ApiResponse<Translator>> Update(Translator translator)
        {
            var request = new RestRequest("/Translator/Update", Method.Put);
            var payload = new
            {
                translator.Id,
                translator.Name
            };
            request.AddBody(payload);
            var response = await Client.ExecuteAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<Translator>(true, translator, null);
            }
            else if (response.Content is not null)
            {
                var errors = JObject.Parse(response.Content)["Errors"];
                List<(string Error, string Message)> errorsParsed = [];
                if (errors is not null)
                    foreach (var error in errors)
                    {
                        string propertyName = $"{nameof(Translator)}.{error["propertyName"]}";
                        string errorMessage = error["errorMessage"]!.ToString();
                        errorsParsed.Add((propertyName, errorMessage));
                    }

                return new ApiResponse<Translator>(false, null, errorsParsed);
            }
            return new ApiResponse<Translator>(false, null, [("General", "Request failed without Validation errors!")]);
        }
    }
}
