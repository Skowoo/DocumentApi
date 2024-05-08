using DocumentApi.Application.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApi.Web.Controllers
{
    [Route("api/clients")]
    [ApiController]
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
            service.Add(item);
            return Ok(item);
        }

        [HttpPut]
        public IActionResult Update(Client item)
        {
            service.Update(item);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            service.Delete(id);
            return Ok();
        }
    }
}
