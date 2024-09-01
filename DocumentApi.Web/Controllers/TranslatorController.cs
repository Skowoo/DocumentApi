using DocumentApi.Application.Translators.Commands.CreateTranslator;
using DocumentApi.Application.Translators.Commands.DeleteTranslator;
using DocumentApi.Application.Translators.Commands.UpdateTranslator;
using DocumentApi.Application.Translators.Queries.GetAllTranslators;
using DocumentApi.Application.Translators.Queries.GetTranslator;
using DocumentApi.Domain.Constants;
using DocumentApi.Domain.Entities;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApi.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = $"{Roles.User},{Roles.Administrator}")]
    public class TranslatorController(ISender sender) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(List<Translator>), 200)]
        public async Task<IActionResult> GetAll() => Ok(await sender.Send(new GetAllTranslatorsQuery()));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Translator), 200)]
        [ProducesResponseType(typeof(int), 404)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await sender.Send(new GetTranslatorQuery(id));
            return result is not null ? Ok(result) : NotFound(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(ValidationFailure), 400)]
        public async Task<IActionResult> Add(CreateTranslatorCommand command) => Ok(await sender.Send(command));

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ValidationFailure), 400)]
        public async Task<IActionResult> Update(UpdateTranslatorCommand command)
        {
            await sender.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            await sender.Send(new DeleteTranslatorCommand(id));
            return NoContent();
        }
    }
}
