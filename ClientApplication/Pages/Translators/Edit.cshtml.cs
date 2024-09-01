using ClientApplication.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientApplication.Pages.Translators
{
    public class EditModel(IRestService<Translator> translatorService) : PageModel
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

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await translatorService.UpdateAsync(Translator);

            if (result.IsSuccess)
                return RedirectToPage($"/Translators/Details", new { id = result.Data!.Id });
            else
                foreach (var (Property, Message) in result.ErrorDetails!)
                    ModelState.TryAddModelError(Property, Message);

            return Page();
        }
    }
}