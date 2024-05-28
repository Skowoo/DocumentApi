using ClientApplication.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ClientApplication.Pages.AdminPanel
{
    public class LoginModel(CurrentUser user, IOptions<DocumentApiConfig> apiConfig) : PageModel
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
                user.LogInUser(result.Content!);
                return RedirectToPage("/Index");
            }
            else if (result.Content is not null)
            {
                var errors = JsonConvert.DeserializeObject<IEnumerable<IdentityError>>(result.Content);
                foreach (var error in errors!)
                    ModelState.AddModelError(error.Code ??= "Custom", error.Description);
                return Page();
            }

            return Page();            
        }
    }
}
