using ClientApplication.Config;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RestSharp.Authenticators;
using RestSharp;

namespace ClientApplication.Pages.Translators
{
    public class CreateModel(CurrentUser user, IOptions<DocumentApiConfig> apiConfig) : PageModel
    {
        [BindProperty]
        public Translator Translator { get; set; } = default!;

        public void OnGet() => Page();

        public async Task<IActionResult> OnPostAsync()
        {
            var options = new RestClientOptions(apiConfig.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            var client = new RestClient(options);
            var request = new RestRequest("/Translator/Add", Method.Post);
            var payload = new
            {
                Translator.Name
            };
            request.AddBody(payload);
            var response = await client.ExecuteAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Translators/Index");
            }
            else if (response.Content is not null)
            {
                var errors = JObject.Parse(response.Content)["Errors"];
                if (errors is not null)
                    foreach (var error in errors)
                    {
                        string propertyName = $"{nameof(Translator)}.{error["propertyName"]}";
                        string errorMessage = error["errorMessage"]!.ToString();
                        ModelState.TryAddModelError(propertyName, errorMessage);
                    }
            }
            return Page();
        }
    }
}
