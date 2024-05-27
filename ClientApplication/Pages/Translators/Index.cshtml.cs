using ClientApplication.Config;
using ClientApplication.Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientApplication.Pages.Translators
{
    public class IndexModel(CurrentUser user) : PageModel
    {
        public void OnGet()
        {
            if (!user.IsInRole(Roles.User))
                RedirectToPage("/Index");
        }
    }
}
