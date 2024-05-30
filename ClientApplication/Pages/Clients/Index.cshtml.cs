using ClientApplication.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientApplication.Pages.Clients
{
    public class IndexModel(IApiClientService clientService) : PageModel
    {
        public List<Client> ClientsList { get; private set; } = [];
        public Client BaseEntity = new();

        public async Task OnGetAsync()
        {
            var result = await clientService.GetAll();
            if (result.Success)
                ClientsList = result.Value!;
            else
                foreach (var error in result.Errors!)
                    ModelState.TryAddModelError(error.Property, error.Message);
        }
    }
}
