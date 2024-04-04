using DocumentApi.Application.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApi.Web.Controllers
{
    [Route("api/translators")]
    [ApiController]
    public class TranslatorController(ITranslatorService service) : ControllerBase
    {
        [HttpGet]
        public List<Translator> GetAll() => service.GetAll();

        [HttpGet("{id}")]
        public Translator? GetById (int id) => service.GetById(id);
    }
}
