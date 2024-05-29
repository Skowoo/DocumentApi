using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using RestSharp.Authenticators;
using RestSharp;
using ClientApplication.Config;
using Microsoft.Extensions.Options;
using DocumentApi.Domain.Constants;

namespace ClientApplication.Pages.Clients
{
    public class DeleteModel(CurrentUser user, IOptions<DocumentApiConfig> apiConfig) : PageModel
    {
        [BindProperty]
        public Client Client { get; set; } = default!;

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
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                var downloadedEntity = JsonConvert.DeserializeObject<Client>(response.Content)!;
                if (downloadedEntity is null)
                    ModelState.TryAddModelError("Client", "Client not found");
                else
                    Client = downloadedEntity;
            }
        }

        public IActionResult OnPost(int id)
        {
            var options = new RestClientOptions(apiConfig.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/Client/Delete/{id}", Method.Delete);
            var response = client.ExecuteAsync(request, Method.Delete).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Clients/Index");
            }
            return Page();
        }
    }
}
