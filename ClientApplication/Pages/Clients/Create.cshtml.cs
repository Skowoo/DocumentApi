using ClientApplication.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientApplication.Pages.Clients
{
    public class CreateModel(IApiClientService clientService) : PageModel
    {
        [BindProperty]
        public Client Client { get; set; } = default!;

        public void OnGet() => Page();

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await clientService.Create(Client);
            if (result.Success)            
                return RedirectToPage("/Clients/Index");              
            else
                foreach (var error in result.Errors!)
                    ModelState.TryAddModelError(error.Property, error.Message);

            return Page();
        }
    }
}
