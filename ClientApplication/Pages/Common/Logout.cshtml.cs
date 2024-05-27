using ClientApplication.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientApplication.Pages.Common
{
    public class LogoutModel(CurrentUser user) : PageModel
    {
        [BindProperty]
        public string? Login { get; set; }

        public IActionResult OnGet()
        {
            Login = user.Login!;
            user.LogOutUser();
            return RedirectToPage("/Common/Login");
        }
    }
}
