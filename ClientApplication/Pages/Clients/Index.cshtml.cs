using ClientApplication.Config;
using ClientApplication.Domain;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace ClientApplication.Pages.Clients
{
    public class IndexModel(CurrentUser user, IOptions<DocumentApiConfig> apiConfig) : PageModel
    {
        public List<Client> ClientsList { get; private set; } = [];
        public Client BaseEntity = new();

        public void OnGet()
        {
            if (!user.IsInRole(Roles.User))
                RedirectToPage("/Index");

            var options = new RestClientOptions(apiConfig.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            var client = new RestClient(options);
            var request = new RestRequest("/Client/GetAll", Method.Get);
            var response = client.ExecuteAsync(request).Result;
            if (response.Content is not null && response.IsSuccessStatusCode)
            {
                ClientsList = JsonConvert.DeserializeObject<List<Client>>(response.Content)!;
            }
        }
    }
}