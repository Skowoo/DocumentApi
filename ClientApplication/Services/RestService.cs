using ClientApplication.Classes;
using ClientApplication.Config;
using ClientApplication.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System.Reflection;

namespace ClientApplication.Services
{
    public class RestService<TItem> : IRestService<TItem> where TItem : class
    {
        readonly RestClient Client;

        public RestService(CurrentUser user, IOptions<DocumentApiConfig> config)
        {
            var options = new RestClientOptions(config.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            Client = new RestClient(options);
        }

        public async Task<ApiResponse<TItem>> CreateAsync(TItem input)
        {
            var request = new RestRequest($"/{typeof(TItem).Name}/Add", Method.Post);
            var payload = System.Text.Json.JsonSerializer.Serialize(input!);
            request.AddBody(payload);

            var response = await Client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {
                PropertyInfo propertyInfo = input!.GetType().GetProperty("Id")!;
                Type idType = propertyInfo.PropertyType;
                object idValue = JsonConvert.DeserializeObject(response.Content!, idType)!;
                propertyInfo.SetValue(input, idValue);

                return new ApiResponse<TItem>(true, input, null);
            }
            else if (response.Content is not null)
            {
                var errors = JObject.Parse(response.Content)["Errors"];
                List<(string Error, string Message)> errorsParsed = [];
                if (errors is not null)
                {
                    foreach (var error in errors)
                    {
                        string propertyName = $"{typeof(TItem).Name}.{error["propertyName"]}";
                        string errorMessage = error["errorMessage"]!.ToString();
                        errorsParsed.Add((propertyName, errorMessage));
                    }
                    return new ApiResponse<TItem>(false, null, errorsParsed);
                }
            }
            return new ApiResponse<TItem>(false, null, [("General", "Request failed without Validation errors!")]);
        }

        public async Task<ApiResponse<TItem>> DeleteAsync(int id) => await DeleteAsync(id.ToString());

        public async Task<ApiResponse<TItem>> DeleteAsync(string id)
        {
            var request = new RestRequest($"/{typeof(TItem).Name}/Delete/{id}", Method.Delete);

            var response = await Client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
                return new ApiResponse<TItem>(true, null, null);

            return new ApiResponse<TItem>(false, null, [("General", "Request failed!")]);
        }

        public async Task<ApiResponse<List<TItem>>> GetAllAsync()
        {
            var request = new RestRequest($"/{typeof(TItem).Name}/GetAll", Method.Get);
            var response = await Client.ExecuteAsync(request);
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                var value = JsonConvert.DeserializeObject<List<TItem>>(response.Content!)!;
                return new ApiResponse<List<TItem>>(true, value, null);
            }

            return new ApiResponse<List<TItem>>(true, null, [("General", "Request Failed!")]);
        }

        public async Task<ApiResponse<TItem>> GetByIdAsync(string id)
        {
            var request = new RestRequest($"{typeof(TItem).Name}/GetById/{id}");
            var response = await Client.ExecuteAsync(request, Method.Get);
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                var downloadedEntity = JsonConvert.DeserializeObject<TItem>(response.Content)!;
                if (downloadedEntity is not null)
                    return new ApiResponse<TItem>(true, downloadedEntity, null);
            }

            return new ApiResponse<TItem>(false, null, [("General", "Translator not found!")]);
        }

        public async Task<ApiResponse<TItem>> GetByIdAsync(int id) => await GetByIdAsync(id.ToString());

        public async Task<ApiResponse<TItem>> UpdateAsync(TItem input)
        {
            var request = new RestRequest($"/{typeof(TItem).Name}/Update", Method.Put);
            var payload = System.Text.Json.JsonSerializer.Serialize(input!);
            request.AddBody(payload);

            var response = await Client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<TItem>(true, input, null);
            }
            else if (response.Content is not null)
            {
                var errors = JObject.Parse(response.Content)["Errors"];
                List<(string Error, string Message)> errorsParsed = [];
                if (errors is not null)
                    foreach (var error in errors)
                    {
                        string propertyName = $"{typeof(TItem).Name}.{error["propertyName"]}";
                        string errorMessage = error["errorMessage"]!.ToString();
                        errorsParsed.Add((propertyName, errorMessage));
                    }

                return new ApiResponse<TItem>(false, null, errorsParsed);
            }
            return new ApiResponse<TItem>(false, null, [("General", "Request failed without Validation errors!")]);
        }
    }
}
