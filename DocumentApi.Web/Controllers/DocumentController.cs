using DocumentApi.Application.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApi.Web.Controllers
{
    [Route("api/documents/")] // Route to controller
    [ApiController]
    [Authorize]
    public class DocumentController(IDocumentService service) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll() => Ok(service.GetAll());

        [HttpGet("{id}")] // Endpoint adress - in parenthesis additional route element to avoid targeting multiple endpoints at once
        public IActionResult GetById(Guid id)
        {
            var item = service.GetById(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public IActionResult Add(Document item)
        {
            if (item is null)
                return BadRequest(item);

            service.Add(item);
            return Created(Url.Action("Get", new { id = item.Id }), item);
        }

        [HttpPut]
        public IActionResult Update(Document item)
        {
            var target = service.GetById(item.Id);

            if (target is null)
                return NotFound();

            service.Update(target);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var target = service.GetById(id);

            if (target is null)
                return NotFound();

            service.Delete(target.Id);

            return NoContent();
        }
    }
}
