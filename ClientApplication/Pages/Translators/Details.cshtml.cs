using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientApplication.Interfaces;

namespace ClientApplication.Pages.Translators
{
    public class DetailsModel(IApiTranslatorService translatorService) : PageModel
    {
        [BindProperty]
        public Translator Translator { get; set; } = default!;

        public async Task OnGetAsync(int id)
        {
            var result = await translatorService.GetById(id);
            if (result.Success)
                Translator = result.Value!;
            else
                foreach (var (Property, Message) in result.Errors!)
                    ModelState.TryAddModelError(Property, Message);
        }
    }
}
