using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientApplication.Interfaces;

namespace ClientApplication.Pages.Documents
{
    public class DeleteModel(IApiDocumentService documentService) : PageModel
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

        public async Task<IActionResult> OnPostAsync(string id)
        {            
            var result = await documentService.Delete(id);
            if (result.Success)
                return RedirectToPage("/Documents/Index");
            else
                foreach (var error in result.Errors!)
                    ModelState.AddModelError(error.Property, error.Message);

            return Page();
        }
    }
}
