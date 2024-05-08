using DocumentApi.Application.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace DocumentApi.Web.Controllers
{
    [Route("api/translators")]
    [ApiController]
    public class TranslatorController(ITranslatorService service) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll() => Ok(service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById (int id)
        {
            var item = service.GetById(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public IActionResult Add(Translator item)
        {
            service.Add(item);
            return Ok(item);
        }

        [HttpPut]
        public IActionResult Update(Translator document)
        {
            try
            {
                service.Update(document);
                return Ok();
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                service.Delete(id);
                return Ok();
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
