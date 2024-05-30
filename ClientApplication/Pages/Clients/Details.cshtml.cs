using ClientApplication.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientApplication.Pages.Clients
{
    public class DetailsModel(IApiClientService clientService) : PageModel
    {
        public Client Client = new();

        public async Task OnGetAsync(int id)
        {
            var result = await clientService.GetById(id);

            if (result.Success)
                Client = result.Value!;
            else
                foreach (var error in result.Errors!)
                    ModelState.TryAddModelError(error.Property, error.Message);
        }
    }
}
