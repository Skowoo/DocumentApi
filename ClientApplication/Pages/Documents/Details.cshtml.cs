using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocumentApi.Domain.Entities;
using ClientApplication.Services;

namespace ClientApplication.Pages.Documents
{
    public class DetailsModel(IRestService<Document> documentService) : PageModel
    {
        [BindProperty]
        public Document Document { get; set; } = default!;

        public async Task OnGetAsync(string id)
        {
            var result = await documentService.GetByIdAsync(id);
            if (result.IsSuccess)
                Document = result.Data!;
            else
                foreach (var (Property, Message) in result.ErrorDetails!)
                    ModelState.AddModelError(Property, Message);
        }
    }
}
