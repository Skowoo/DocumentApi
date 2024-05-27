using ClientApplication.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ClientApplication.Pages.AdminPanel
{
    public class IndexModel(JwtTokenStorage tokenStorage, IOptions<DocumentApiConfig> apiConfig) : PageModel
    {
        public void OnGet() => Page();

        public IActionResult OnPost()
        {
            var client = new RestClient(apiConfig.Value.FullApiUri);
            var request = new RestRequest("/identity/login");
            var payload = new JObject
            {
                ["login"] = Request.Form["login"].ToString(),
                ["password"] = Request.Form["password"].ToString()
            };            
            request.AddStringBody(payload.ToString(), DataFormat.Json);

            var result = client.ExecutePostAsync(request).Result;
            if (result.IsSuccessStatusCode)
            {
                tokenStorage.SetToken(result.Content!);
            }

            var token = tokenStorage.GetToken();

            return RedirectToPage("/Index");
        }
    }
}
