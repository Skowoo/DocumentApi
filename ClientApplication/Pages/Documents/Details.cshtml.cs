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
            var result = await documentService.GetById(id);
            if (result.Success)
                Document = result.Value!;
            else
                foreach (var error in result.Errors!)
                    ModelState.AddModelError(error.Property, error.Message);
        }
    }
}
