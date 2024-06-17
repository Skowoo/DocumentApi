using ClientApplication.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientApplication.Pages.Clients
{
    public class IndexModel(IRestService<Client> clientService) : PageModel
    {
        public List<Client> ClientsList { get; private set; } = [];
        public Client BaseEntity = new();

        public async Task OnGetAsync()
        {
            var result = await clientService.GetAllAsync();
            if (result.IsSuccess)
                ClientsList = result.Data!;
            else
                foreach (var error in result.ErrorDetails!)
                    ModelState.TryAddModelError(error.Property, error.Message);
        }
    }
}
