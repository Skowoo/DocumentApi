using ClientApplication.Config;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace ClientApplication.Services
{
    public class IdentityRestService : IIdentityRestService
    {
        readonly RestClient restClient;

        public IdentityRestService(IOptions<DocumentApiConfig> config, CurrentUser user)
        {
            var options = new RestClientOptions($"{config.Value.FullApiUri}/Identity")
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            restClient = new RestClient(options);
        }

        public async Task<(IdentityResult Result, string Id)> CreateUser(AppUser user)
        {
            var request = new RestRequest("Register", Method.Post);
            request.AddJsonBody(user);
            var response = await restClient.ExecuteAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var parsedId = JsonConvert.DeserializeObject<string>(response.Content!);
                return (IdentityResult.Success, parsedId!);
            }
            else
            {
                return (JsonConvert.DeserializeObject<IdentityResult>(response.Content!)!, "");
            }
        }

        public async Task<IdentityResult> AssignUserToRole(string userName, string roleName)
        {
            var request = new RestRequest("AddUserToRole", Method.Post);
            var payload = System.Text.Json.JsonSerializer.Serialize(new AppUser { Login = userName, Password = roleName});
            request.AddJsonBody(payload);
            var response = await restClient.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<IdentityResult>(response.Content!)!;
        }

        public async Task<IdentityResult> RemoveUserFromRole(string userName, string roleName)
        {
            var request = new RestRequest("RemoveUserFromRole", Method.Delete);
            var payload = System.Text.Json.JsonSerializer.Serialize(new AppUser { Login = userName, Password = roleName });
            request.AddJsonBody(payload);
            var response = await restClient.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<IdentityResult>(response.Content!)!;
        }

        public async Task<List<IdentityUser>> GetAllUsers()
        {
            var request = new RestRequest("GetAllUsers", Method.Get);
            var response = await restClient.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<List<IdentityUser>>(response.Content!)!;
        }

        public async Task<IdentityUser?> GetUserById(Guid Id)
        {
            var request = new RestRequest($"GetUserById/{Id}", Method.Get);
            var response = await restClient.ExecuteAsync(request);
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<IdentityUser>(response.Content!)!;
            else
                return null;
        }

        public async Task<List<IdentityRole>> GetAllRoles() 
        {
            var request = new RestRequest("GetAllRoles", Method.Get);
            var response = await restClient.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<List<IdentityRole>>(response.Content!)!;
        }
    }
}
