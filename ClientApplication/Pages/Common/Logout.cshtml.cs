using ClientApplication.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientApplication.Pages.Common
{
    public class LogoutModel(CurrentUser user) : PageModel
    {
        public IActionResult OnGet()
        {
            user.Logout();
            return RedirectToPage("/Common/Login");
        }
    }
}
