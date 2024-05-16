using DocumentApi.Application.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApi.Web.Controllers
{
    [Route("api/clients")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ClientController(IClientService service) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll() => Ok(service.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = service.GetById(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public IActionResult Add(Client item)
        {
            if (item is null)
                return BadRequest(item);

            service.Add(item);
            return Created(Url.Action("Get", new { id = item.Id }), item);
        }

        [HttpPut]
        public IActionResult Update(Client item)
        {
            var target = service.GetById(item.Id);

            if (target is null)
                return NotFound();

            service.Update(target);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var target = service.GetById(id);

            if (target is null)
                return NotFound();

            service.Delete(target.Id);

            return NoContent();
        }
    }
}
