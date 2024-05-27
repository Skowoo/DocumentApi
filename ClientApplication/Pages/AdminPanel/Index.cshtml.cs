using ClientApplication.Config;
using ClientApplication.Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientApplication.Pages.AdminPanel
{
    public class IndexModel(CurrentUser user) : PageModel
    {
        public void OnGet()
        {
            if (!user.IsInRole(Roles.Administrator))
                RedirectToPage("/Index");
        }
    }
}
