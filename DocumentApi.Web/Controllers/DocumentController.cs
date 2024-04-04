using DocumentApi.Application.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApi.Web.Controllers
{
    [Route("api/documents/")] // Route to controller
    [ApiController]
    public class DocumentController(IDocumentService service) : ControllerBase
    {
        [HttpGet]
        public List<Document> GetAll() => service.GetAll();

        [HttpGet("{id}")] // Endpoint adress - in parenthesis additional route element to avoid targeting multiple endpoints at once
        public Document? GetById(Guid id) => service.GetById(id);
    }
}
