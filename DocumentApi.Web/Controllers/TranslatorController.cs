using DocumentApi.Application.Translators.Commands.CreateTranslator;
using DocumentApi.Application.Translators.Commands.DeleteTranslator;
using DocumentApi.Application.Translators.Commands.UpdateTranslator;
using DocumentApi.Application.Translators.Queries.GetAllTranslators;
using DocumentApi.Application.Translators.Queries.GetTranslator;
using DocumentApi.Domain.Constants;
using DocumentApi.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApi.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = $"{Roles.User},{Roles.Administrator}")]
    public class TranslatorController : ControllerBase
    {
        [HttpGet]
        public Task<List<Translator>> GetAll(ISender sender) => sender.Send(new GetAllTranslatorsQuery());

        [HttpGet("{id}")]
        public Task<Translator?> GetById (ISender sender, int id) => sender.Send(new GetTranslatorQuery(id));

        [HttpPost]
        public Task<int> Add(ISender sender, CreateTranslatorCommand command) => sender.Send(command);

        [HttpPut]
        public Task Update(ISender sender, UpdateTranslatorCommand command) => sender.Send(command);

        [HttpDelete("{id}")]
        public Task Delete(ISender sender, int id) => sender.Send(new DeleteTranslatorCommand(id));
    }
}
