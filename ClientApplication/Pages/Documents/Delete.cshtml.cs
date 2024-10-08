using ClientApplication.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientApplication.Pages.Documents
{
    public class DeleteModel(IRestService<Document> documentService) : PageModel
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            var result = await documentService.DeleteAsync(id);
            if (result.IsSuccess)
                return RedirectToPage("/Documents/Index");
            else
                foreach (var error in result.ErrorDetails!)
                    ModelState.AddModelError(error.Property, error.Message);

            return Page();
        }
    }
}
