using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientApplication.Interfaces;

namespace ClientApplication.Pages.Documents
{
    public class IndexModel(IApiDocumentService documentService) : PageModel
    {
        public Document BaseEntity { get; set; } = default!;
        public List<Document> DocumentsList { get; set; } = [];

        public async Task OnGetAsync()
        {
            var result = await documentService.GetAllAsync();
            if (result.IsSuccess)
                DocumentsList = result.Data!;
            else
                foreach (var error in result.ErrorDetails!)
                    ModelState.AddModelError(error.Property, error.Message);
        }
    }
}