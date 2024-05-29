using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientApplication.Interfaces;

namespace ClientApplication.Pages.Translators
{
    public class CreateModel(IApiTranslatorService translatorService) : PageModel
    {
        [BindProperty]
        public Translator Translator { get; set; } = default!;

        public void OnGet() => Page();

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await translatorService.Create(Translator);

            if (result.Success)
                return RedirectToPage("/Translators/Index");
            else
                foreach (var (Property, Message) in result.Errors!)
                    ModelState.TryAddModelError(Property, Message);

            return Page();
        }
    }
}
