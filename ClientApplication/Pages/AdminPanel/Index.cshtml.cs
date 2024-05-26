using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ClientApplication.Pages.AdminPanel
{
    public class IndexModel : PageModel
    {
        public void OnGet() => Page();

        public IActionResult OnPost()
        {
            var client = new RestClient("https://localhost:7176/api/identity/login");
            var payload = new JObject
            {
                ["login"] = Request.Form["login"].ToString(),
                ["password"] = Request.Form["password"].ToString()
            };
            var request = new RestRequest();
            request.AddStringBody(payload.ToString(), DataFormat.Json);

            var result = client.ExecutePostAsync(request).Result;

            return RedirectToPage("/Index");
        }
    }
}
