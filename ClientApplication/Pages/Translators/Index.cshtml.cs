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
            var result = await translatorService.GetAllAsync();
            if (result.IsSuccess)
                Translators = result.Data!;
            else
                foreach (var (Property, Message) in result.ErrorDetails!)
                    ModelState.TryAddModelError(Property, Message);
        }
    }
}
