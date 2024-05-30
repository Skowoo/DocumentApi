using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClientApplication.Services;

namespace ClientApplication.Pages.Documents
{
    public class CreateModel(IRestService<Document> documentService, IRestService<Translator> translatorService, IRestService<Client> clientService) : PageModel
    {
        [BindProperty]
        public Document Document { get; set; } = default!;

        public SelectList ClientsList { get; set; } = default!;

        public SelectList TranslatorsList { get; set; } = default!;

        public async void OnGet() 
        {
            var translatorsResult = await translatorService.GetAllAsync();
            TranslatorsList = new SelectList(translatorsResult.Data, nameof(Translator.Id), nameof(Translator.Name));

            var clientsResult = await clientService.GetAllAsync();
            ClientsList = new SelectList(clientsResult.Data, nameof(Client.Id), nameof(Client.Name));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Document.CreatedAt = DateTime.Now;

            var response = await documentService.CreateAsync(Document);

            if (response.IsSuccess)
            {
                return RedirectToPage("/Documents/Index");
            }
            else
            {
                foreach (var error in response.ErrorDetails!)
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
