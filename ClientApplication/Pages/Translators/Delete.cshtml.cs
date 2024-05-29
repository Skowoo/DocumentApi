using ClientApplication.Config;
using DocumentApi.Domain.Constants;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp.Authenticators;
using RestSharp;

namespace ClientApplication.Pages.Translators
{
    public class DeleteModel(CurrentUser user, IOptions<DocumentApiConfig> apiConfig) : PageModel
    {
        [BindProperty]
        public Translator Translator { get; set; } = default!;

        public void OnGet(int id)
        {
            if (!user.IsInRole(Roles.User))
                RedirectToPage("/Index");

            var options = new RestClientOptions(apiConfig.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            var client = new RestClient(options);
            var request = new RestRequest($"Translator/GetById/{id}");
            var response = client.ExecuteAsync(request, Method.Get).Result;
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                var downloadedEntity = JsonConvert.DeserializeObject<Translator>(response.Content)!;
                if (downloadedEntity is null)
                    ModelState.TryAddModelError("Translator", "Translator not found");
                else
                    Translator = downloadedEntity;
            }
        }

        public IActionResult OnPost(int id)
        {
            var options = new RestClientOptions(apiConfig.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            var client = new RestClient(options);
            var request = new RestRequest($"/Translator/Delete/{id}", Method.Delete);
            var response = client.ExecuteAsync(request, Method.Delete).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Translators/Index");
            }
            return Page();
        }
    }
}
