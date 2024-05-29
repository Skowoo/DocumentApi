using ClientApplication.Config;
using DocumentApi.Domain.Constants;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using RestSharp.Authenticators;
using RestSharp;
using Microsoft.Extensions.Options;

namespace ClientApplication.Pages.Documents
{
    public class IndexModel(CurrentUser user, IOptions<DocumentApiConfig> apiConfig) : PageModel
    {
        public Document BaseEntity { get; set; } = default!;
        public List<Document> DocumentsList { get; set; } = [];

        public void OnGet()
        {
            if (!user.IsInRole(Roles.User))
                RedirectToPage("/Index");

            var options = new RestClientOptions(apiConfig.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            var client = new RestClient(options);
            var request = new RestRequest("/Document/GetAll", Method.Get);
            var response = client.ExecuteAsync(request).Result;
            if (response.Content is not null && response.IsSuccessStatusCode)
            {
                DocumentsList = JsonConvert.DeserializeObject<List<Document>>(response.Content)!;
            }
        }
    }
}
