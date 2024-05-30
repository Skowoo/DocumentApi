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
    public class ApiDocumentService : IApiDocumentService
    {
        RestClient Client { get; set; }

        public ApiDocumentService(CurrentUser user, IOptions<DocumentApiConfig> config)
        {
            var options = new RestClientOptions(config.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            Client = new RestClient(options);
        }

        public async Task<ApiResponse<Document>> Create(Document document)
        {
            var request = new RestRequest("/Document/Add", Method.Post);
            var payload = new
            {
                document.Title,
                document.SignsSize,
                document.CreatedAt,
                document.Deadline,
                document.ClientId,
                document.TranslatorId
            };
            request.AddBody(payload);
            var response = await Client.ExecuteAsync(request);
            if (response.IsSuccessStatusCode)
            {
                document.Id = JsonConvert.DeserializeObject<Guid>(response.Content!);
                return new ApiResponse<Document>(true, document, null);
            }
            else if (response.Content is not null)
            {
                var errors = JObject.Parse(response.Content)["Errors"];
                List<(string Error, string Message)> errorsParsed = [];
                if (errors is not null)
                    foreach (var error in errors)
                    {
                        string propertyName = $"{nameof(Document)}.{error["propertyName"]}";
                        string errorMessage = error["errorMessage"]!.ToString();
                        errorsParsed.Add((propertyName, errorMessage));
                    }

                return new ApiResponse<Document>(false, null, errorsParsed);
            }
            return new ApiResponse<Document>(false, null, [("General", "Request failed without Validation errors!")]);
        }

        public async Task<ApiResponse<Document>> Delete(string id)
        {
            var request = new RestRequest($"/Document/Delete/{id}", Method.Delete);

            var response = await Client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
                return new ApiResponse<Document>(true, null, null);

            return new ApiResponse<Document>(false, null, [("General", "Request failed!")]);
        }

        public async Task<ApiResponse<List<Document>>> GetAll()
        {
            var request = new RestRequest("/Document/GetAll", Method.Get);
            var response = await Client.ExecuteAsync(request);
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                var value = JsonConvert.DeserializeObject<List<Document>>(response.Content!)!;
                return new ApiResponse<List<Document>>(true, value, null);
            }

            return new ApiResponse<List<Document>>(true, null, [("General", "Request Failed!")]);            
        }

        public async Task<ApiResponse<Document>> GetById(string id)
        {
            var request = new RestRequest($"Document/GetById/{id}");
            var response = await Client.ExecuteAsync(request, Method.Get);
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                var downloadedEntity = JsonConvert.DeserializeObject<Document>(response.Content)!;
                if (downloadedEntity is not null)
                    return new ApiResponse<Document>(true, downloadedEntity, null);
            }
            return new ApiResponse<Document>(false, null, [("General", "Document not found!")]);
        }

        public async Task<ApiResponse<Document>> Update(Document document)
        {
            var request = new RestRequest("/Document/Update", Method.Put);

            var payload = new
            {
                document.Id,
                document.Title,
                document.SignsSize,
                document.CreatedAt,
                document.Deadline,
                document.ClientId,
                document.TranslatorId
            };
            request.AddBody(payload);

            var response = await Client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return new ApiResponse<Document>(true, document, null);
            }
            else if (response.Content is not null)
            {
                var errors = JObject.Parse(response.Content)["Errors"];
                List<(string Error, string Message)> errorsParsed = [];
                if (errors is not null)
                    foreach (var error in errors)
                    {
                        string propertyName = $"{nameof(Document)}.{error["propertyName"]}";
                        string errorMessage = error["errorMessage"]!.ToString();
                        errorsParsed.Add((propertyName, errorMessage));
                    }

                return new ApiResponse<Document>(false, null, errorsParsed);
            }
            return new ApiResponse<Document>(false, null, [("General", "Request failed without Validation errors!")]);
        }
    }
}
