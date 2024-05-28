using ClientApplication.Config;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp.Authenticators;
using RestSharp;
using ClientApplication.Domain;

namespace ClientApplication.Pages.Clients
{
    public class EditModel(CurrentUser user, IOptions<DocumentApiConfig> apiConfig) : PageModel
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

        public async Task<IActionResult> OnPostAsync()
        {
            var options = new RestClientOptions(apiConfig.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            var client = new RestClient(options);
            var request = new RestRequest("/Client/Update", Method.Put);
            var payload = new
            {
                Client.Id,
                Client.Name,
                Client.Email,
                Client.TelephoneNumber,
            };
            request.AddBody(payload);
            var response = await client.ExecuteAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage($"/Clients/Details", new { id = Client.Id });
            }
            else
            {
                return Page();
            }
        }
    }
}
