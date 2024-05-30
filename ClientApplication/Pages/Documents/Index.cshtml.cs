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
            var result = await documentService.GetAll();
            if (result.Success)
                DocumentsList = result.Value!;
            else
                foreach (var error in result.Errors!)
                    ModelState.AddModelError(error.Property, error.Message);
        }
    }
}