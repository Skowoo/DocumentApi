using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientApplication.Interfaces;

namespace ClientApplication.Pages.Translators
{
    public class DeleteModel(IApiTranslatorService translatorService) : PageModel
    {
        [BindProperty]
        public Translator Translator { get; set; } = default!;

        public async Task OnGetAsync(int id)
        {
            var result = await translatorService.GetByIdAsync(id);
            if (result.IsSuccess)
                Translator = result.Data!;
            else
                foreach (var (Property, Message) in result.ErrorDetails!)
                    ModelState.TryAddModelError(Property, Message);
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var result = await translatorService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return RedirectToPage("/Translators/Index");
            }
            return Page();
        }
    }
}
