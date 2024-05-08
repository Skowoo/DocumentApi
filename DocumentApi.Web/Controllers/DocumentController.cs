﻿using DocumentApi.Application.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApi.Web.Controllers
{
    [Route("api/documents/")] // Route to controller
    [ApiController]
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
            service.Add(item);
            return Created();
        }

        [HttpPut]
        public IActionResult Update(Document item)
        {
            try
            {
                service.Update(item);
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
        public IActionResult Delete(Guid id)
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
