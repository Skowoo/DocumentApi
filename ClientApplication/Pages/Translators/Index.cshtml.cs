using ClientApplication.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientApplication.Pages.Translators
{
    public class IndexModel(IApiTranslatorService translatorService) : PageModel
    {
        public List<Translator> Translators { get; set; } = default!;
        public Translator BaseEntity { get; set; } = default!;

        public async Task OnGet()
        {
            var result = await translatorService.GetAll();
            if (result.Success)
                Translators = result.Value!;
            else
                foreach (var (Property, Message) in result.Errors!)
                    ModelState.TryAddModelError(Property, Message);
        }
    }
}
