using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientApplication.Pages.AdminPanel
{
    public class IndexModel : PageModel
    {
        public void OnGet() => Page();

        public IActionResult OnPost()
        {
            return RedirectToPage("/Index");
        }
    }
}
