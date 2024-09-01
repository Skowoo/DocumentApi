using ClientApplication.Classes;
using ClientApplication.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClientApplication.Pages.Documents
{
    public class CreateModel(IRestService<Document> documentService, SelectListHelper selectListHelper) : PageModel
    {
        [BindProperty]
        public Document Document { get; set; } = default!;

        public SelectList ClientsList { get; set; } = default!;

        public SelectList TranslatorsList { get; set; } = default!;

        public async Task OnGetAsync() => await PopulateListsAsync();

        public async Task<IActionResult> OnPostAsync()
        {
            if (Document.TranslatorId == 0)
                Document.TranslatorId = null;

            var response = await documentService.CreateAsync(Document);

            if (response.IsSuccess)
                return RedirectToPage("/Documents/Index");

            foreach (var (Property, Message) in response.ErrorDetails!)
                ModelState.AddModelError(Property, Message);

            await PopulateListsAsync();

            return Page();
        }

        async Task PopulateListsAsync()
        {
            TranslatorsList = await selectListHelper.GetTranslatorsSelectListAsync();
            ClientsList = await selectListHelper.GetClientsSelectListAsync();
        }
    }
}
