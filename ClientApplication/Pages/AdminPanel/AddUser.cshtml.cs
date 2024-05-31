using ClientApplication.Services;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientApplication.Pages.AdminPanel
{
    public class AddUserModel(IIdentityRestService identityService) : PageModel
    {
        [BindProperty]
        public AppUser NewUser { get; set; } = default!;

        public void OnGet() => Page();

        public async Task<IActionResult> OnPostAsync()
        {
            var (Result, _) = await identityService.CreateUser(NewUser); // TBD: Should redirect to user page

            if (Result.Succeeded)
                RedirectToPage("/AdminPanel/Index");
            else
                foreach (var error in Result.Errors)
                    ModelState.AddModelError("", error.Description);

            return Page();
        }
    }
}
