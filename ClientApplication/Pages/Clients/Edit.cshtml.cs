using ClientApplication.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientApplication.Pages.Clients
{
    public class EditModel(IRestService<Client> clientService) : PageModel
    {
        [BindProperty]
        public Client Client { get; set; } = default!;

        public async Task OnGetAsync(int id)
        {
            var result = await clientService.GetByIdAsync(id);

            if (result.IsSuccess)
                Client = result.Data!;
            else
                foreach (var error in result.ErrorDetails!)
                    ModelState.TryAddModelError(error.Property, error.Message);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await clientService.UpdateAsync(Client);
            if (result.IsSuccess)
                return RedirectToPage($"/Clients/Details", new { id = result.Data!.Id });
            else
                foreach (var error in result.ErrorDetails!)
                    ModelState.TryAddModelError(error.Property, error.Message);

            return Page();
        }
    }
}
