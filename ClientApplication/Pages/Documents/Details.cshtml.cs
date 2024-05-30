using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocumentApi.Domain.Entities;
using ClientApplication.Interfaces;

namespace ClientApplication.Pages.Documents
{
    public class DetailsModel(IApiDocumentService documentService) : PageModel
    {
        [BindProperty]
        public Document Document { get; set; } = default!;

        public async Task OnGetAsync(string id)
        {
            var result = await documentService.GetByIdAsync(id);
            if (result.IsSuccess)
                Document = result.Data!;
            else
                foreach (var error in result.ErrorDetails!)
                    ModelState.AddModelError(error.Property, error.Message);
        }
    }
}