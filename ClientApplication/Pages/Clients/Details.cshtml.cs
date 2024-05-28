using ClientApplication.Config;
using DocumentApi.Domain.Constants;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace ClientApplication.Pages.Clients
{
    public class DetailsModel(CurrentUser user, IOptions<DocumentApiConfig> apiConfig) : PageModel
    {
        public Client Entity = new();

        public void OnGet(int id)
        {
            if (!user.IsInRole(Roles.User))
                RedirectToPage("/Index");

            var options = new RestClientOptions(apiConfig.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            var client = new RestClient(options);
            var request = new RestRequest($"Client/GetById/{id}");
            var response = client.ExecuteAsync(request, Method.Get).Result;
            if(response.IsSuccessStatusCode && response.Content is not null)
            {
                var downloadedEntity = JsonConvert.DeserializeObject<Client>(response.Content)!;
                if (downloadedEntity is null)
                    ModelState.TryAddModelError("Client", "Client not found");
                else
                    Entity = downloadedEntity;
            }
        }
    }
}
