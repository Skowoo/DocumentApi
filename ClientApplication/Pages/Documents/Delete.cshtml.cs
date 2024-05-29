using DocumentApi.Domain.Constants;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using RestSharp.Authenticators;
using RestSharp;
using ClientApplication.Config;
using Microsoft.Extensions.Options;

namespace ClientApplication.Pages.Documents
{
    public class DeleteModel(CurrentUser user, IOptions<DocumentApiConfig> apiConfig) : PageModel
    {
        [BindProperty]
        public Document CurrentEntity { get; set; } = default!;

        public void OnGet(string id)
        {
            if (!user.IsInRole(Roles.User))
                RedirectToPage("/Index");

            var options = new RestClientOptions(apiConfig.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            var client = new RestClient(options);
            var request = new RestRequest($"Document/GetById/{id}");
            var response = client.ExecuteAsync(request, Method.Get).Result;
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                var downloadedEntity = JsonConvert.DeserializeObject<Document>(response.Content)!;
                if (downloadedEntity is null)
                    ModelState.TryAddModelError("Client", "Client not found");
                else
                    CurrentEntity = downloadedEntity;
            }
        }

        public IActionResult OnPost(string id)
        {
            var options = new RestClientOptions(apiConfig.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/Document/Delete/{id}", Method.Delete);
            var response = client.ExecuteAsync(request, Method.Delete).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Documents/Index");
            }
            return Page();
        }
    }
}
