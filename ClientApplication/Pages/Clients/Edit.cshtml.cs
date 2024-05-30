using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientApplication.Interfaces;

namespace ClientApplication.Pages.Clients
{
    public class EditModel(IApiClientService clientService) : PageModel
    {
        [BindProperty]
        public Client Client { get; set; } = default!;

        public async Task OnGetAsync(int id)
        {
            var result = await clientService.GetById(id);

            if (result.Success)
                Client = result.Value!;
            else
                foreach (var error in result.Errors!)
                    ModelState.TryAddModelError(error.Property, error.Message);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await clientService.Update(Client);
            if (result.Success)
                return RedirectToPage($"/Clients/Details", new { id = result.Value!.Id });
            else
                foreach (var error in result.Errors!)
                    ModelState.TryAddModelError(error.Property, error.Message);

            return Page();
        }
    }
}
