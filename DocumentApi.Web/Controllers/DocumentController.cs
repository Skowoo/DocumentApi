using DocumentApi.Application.Documents.Commands.CreateDocument;
using DocumentApi.Application.Documents.Commands.DeleteDocument;
using DocumentApi.Application.Documents.Commands.UpdateDocument;
using DocumentApi.Application.Documents.Queries.GetAllDocuments;
using DocumentApi.Application.Documents.Queries.GetDocument;
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
    public class DocumentController(ISender sender) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(List<Document>), 200)]
        public async Task<IActionResult> GetAll() => Ok(await sender.Send(new GetAllDocumentsQuery()));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Document), 200)]
        [ProducesResponseType(typeof(Guid), 404)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await sender.Send(new GetDocumentQuery(id));
            return result is not null ? Ok(result) : NotFound(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(ValidationFailure), 400)]
        public async Task<IActionResult> Add(CreateDocumentCommand command) => Ok(await sender.Send(command));

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ValidationFailure), 400)]
        public async Task<IActionResult> Update(UpdateDocumentCommand command)
        {
            await sender.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await sender.Send(new DeleteDocumentCommand(id));
            return NoContent();
        }
    }
}
