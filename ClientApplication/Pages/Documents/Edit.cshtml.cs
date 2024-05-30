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
            var result = await documentService.GetById(id);
            if (result.Success)
                Document = result.Value!;
            else
                foreach (var error in result.Errors!)
                    ModelState.AddModelError(error.Property, error.Message);

            var translatorsResult = await translatorService.GetAll();
            TranslatorsList = new SelectList(translatorsResult.Value, nameof(Translator.Id), nameof(Translator.Name));

            var clientsResult = await clientService.GetAll();
            ClientsList = new SelectList(clientsResult.Value, nameof(Client.Id), nameof(Client.Name));
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var result = await documentService.Update(Document);
            if (result.Success)
            {
                return RedirectToPage("/Documents/Index");
            }
            else
            {
                foreach (var error in result.Errors!)
                    ModelState.AddModelError(error.Property, error.Message);

                var translatorsResult = await translatorService.GetAll();
                TranslatorsList = new SelectList(translatorsResult.Value, nameof(Translator.Id), nameof(Translator.Name));

                var clientsResult = await clientService.GetAll();
                ClientsList = new SelectList(clientsResult.Value, nameof(Client.Id), nameof(Client.Name));
            }
            return Page();
        }
    }
}