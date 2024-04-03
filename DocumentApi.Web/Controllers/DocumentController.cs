using DocumentApi.Application.Interfaces;
using DocumentApi.Domain.Entities;
using DocumentApi.Infrastructure.Data.MemoryService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApi.Web.Controllers
{
    [Route("api/document/")]
    [ApiController]
    public class DocumentController(IDocumentService service) : ControllerBase
    {
        [HttpGet]
        public List<Document> Get() => service.GetAll();
    }
}
