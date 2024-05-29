using ClientApplication.Config;
using DocumentApi.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace ClientApplication.Pages.AdminPanel
{
    public class IndexModel(CurrentUser user, IOptions<DocumentApiConfig> apiConfig) : PageModel
    {
        public SelectList? UsersList { get; set; }

        public void OnGet()
        {
            if (!user.IsInRole(Roles.Administrator))
                RedirectToPage("/Index");


            var options = new RestClientOptions(apiConfig.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            var client = new RestClient(options);
            var request = new RestRequest("/Identity/GetAllUsers", Method.Get);
            var response = client.ExecuteAsync<List<IdentityUser>>(request).Result;
            if (response.IsSuccessStatusCode)
            {
                _ = JsonConvert.DeserializeObject<List<IdentityUser>>(response.Content!);
            }
        }
    }
}
