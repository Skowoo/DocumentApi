using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClientApplication.Interfaces;

namespace ClientApplication.Pages.Documents
{
    public class EditModel(IApiDocumentService documentService, IApiTranslatorService translatorService, IApiClientService clientService) : PageModel
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
                foreach (var error in result.ErrorDetails!)
                    ModelState.AddModelError(error.Property, error.Message);

            var translatorsResult = await translatorService.GetAllAsync();
            TranslatorsList = new SelectList(translatorsResult.Data, nameof(Translator.Id), nameof(Translator.Name));

            var clientsResult = await clientService.GetAllAsync();
            ClientsList = new SelectList(clientsResult.Data, nameof(Client.Id), nameof(Client.Name));
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var result = await documentService.UpdateAsync(Document);
            if (result.IsSuccess)
            {
                return RedirectToPage("/Documents/Index");
            }
            else
            {
                foreach (var error in result.ErrorDetails!)
                    ModelState.AddModelError(error.Property, error.Message);

                var translatorsResult = await translatorService.GetAllAsync();
                TranslatorsList = new SelectList(translatorsResult.Data, nameof(Translator.Id), nameof(Translator.Name));

                var clientsResult = await clientService.GetAllAsync();
                ClientsList = new SelectList(clientsResult.Data, nameof(Client.Id), nameof(Client.Name));
            }
            return Page();
        }
    }
}