using ClientApplication.Classes;
using ClientApplication.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClientApplication.Pages.Documents
{
    public class EditModel(IRestService<Document> documentService, SelectListHelper selectListHelper) : PageModel
    {
        [BindProperty]
        public Document Document { get; set; } = default!;

        public SelectList ClientsList { get; set; } = default!;

        public SelectList TranslatorsList { get; set; } = default!;

        public async Task OnGetAsync(string id)
        {
            var result = await documentService.GetByIdAsync(id);
            if (result.IsSuccess)
                Document = result.Data!;
            else
                foreach (var (Property, Message) in result.ErrorDetails!)
                    ModelState.AddModelError(Property, Message);

            await PopulateSelectLists();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Document.TranslatorId == 0)
                Document.TranslatorId = null;

            var result = await documentService.UpdateAsync(Document);

            if (result.IsSuccess)
                return RedirectToPage("/Documents/Index");

            foreach (var (Property, Message) in result.ErrorDetails!)
                ModelState.AddModelError(Property, Message);

            await PopulateSelectLists();

            return Page();
        }

        async Task PopulateSelectLists()
        {
            TranslatorsList = await selectListHelper.GetTranslatorsSelectListAsync();
            ClientsList = await selectListHelper.GetClientsSelectListAsync();
        }
    }
}