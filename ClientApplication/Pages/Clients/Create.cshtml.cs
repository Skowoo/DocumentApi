using ClientApplication.Config;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;

namespace ClientApplication.Pages.Clients
{
    public class CreateModel(CurrentUser user, IOptions<DocumentApiConfig> apiConfig) : PageModel
    {
        [BindProperty]
        public Client Client { get; set; } = default!;

        public void OnGet() => Page();

        public async Task<IActionResult> OnPostAsync()
        {
            var options = new RestClientOptions(apiConfig.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            var client = new RestClient(options);
            var request = new RestRequest("/Client/Add", Method.Post);
            var payload = new
            {
                Client.Name,
                Client.Email,
                Client.TelephoneNumber,                
            };
            request.AddBody(payload);
            var response = await client.ExecuteAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Clients/Index");
            }                
            else
            {                
                return Page();
            }
        }
    }
}
