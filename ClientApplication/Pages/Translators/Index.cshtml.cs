using ClientApplication.Config;
using DocumentApi.Domain.Constants;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace ClientApplication.Pages.Translators
{
    public class IndexModel(CurrentUser user, IOptions<DocumentApiConfig> apiConfig) : PageModel
    {
        public List<Translator> Translators { get; set; } = default!;
        public Translator BaseEntity { get; set; } = default!;

        public void OnGet()
        {
            if (!user.IsInRole(Roles.User))
                RedirectToPage("/Index");

            var options = new RestClientOptions(apiConfig.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            var client = new RestClient(options);
            var request = new RestRequest("/Translator/GetAll", Method.Get);
            var response = client.ExecuteAsync(request).Result;
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                Translators = JsonConvert.DeserializeObject<List<Translator>>(response.Content!)!;
            }
            else
            {
                ModelState.AddModelError("Api", "Data request failed!");
            }
        }
    }
}
