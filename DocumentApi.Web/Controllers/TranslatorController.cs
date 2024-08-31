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
    public class TranslatorController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public Task<List<Translator>> GetAll() => sender.Send(new GetAllTranslatorsQuery());

        [HttpGet("{id}")]
        public Task<Translator?> GetById (int id) => sender.Send(new GetTranslatorQuery(id));

        [HttpPost]
        public Task<int> Add(CreateTranslatorCommand command) => sender.Send(command);

        [HttpPut]
        public Task Update(UpdateTranslatorCommand command) => sender.Send(command);

        [HttpDelete("{id}")]
        public Task Delete(int id) => sender.Send(new DeleteTranslatorCommand(id));
    }
}
