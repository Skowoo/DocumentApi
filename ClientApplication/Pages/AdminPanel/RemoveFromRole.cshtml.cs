using ClientApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClientApplication.Pages.AdminPanel
{
    public class RemoveFromRoleModel(IIdentityRestService identityService) : PageModel
    {
        [BindProperty]
        public string UserName { get; set; } = default!;

        [BindProperty]
        public string RoleName { get; set; } = default!;

        public SelectList Users { get; set; } = default!;

        public SelectList Roles { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var users = await identityService.GetAllUsers();
            var roles = await identityService.GetAllRoles();
            Users = new SelectList(users, "UserName", "UserName");
            Roles = new SelectList(roles, "Name", "Name");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await identityService.RemoveUserFromRole(UserName, RoleName);
            if (result.Succeeded)
                return RedirectToPage("/AdminPanel/Index");
            else
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description);

            return Page();
        }
    }
}
