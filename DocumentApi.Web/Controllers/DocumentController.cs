using DocumentApi.Application.Documents.Commands.CreateDocument;
using DocumentApi.Application.Documents.Commands.DeleteDocument;
using DocumentApi.Application.Documents.Commands.UpdateDocument;
using DocumentApi.Application.Documents.Queries.GetAllDocuments;
using DocumentApi.Application.Documents.Queries.GetDocument;
using DocumentApi.Domain.Constants;
using DocumentApi.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApi.Web.Controllers
{
    [Route("api/[controller]/[action]")] // Route to controller
    [ApiController]
    [Authorize(Roles = $"{Roles.User},{Roles.Administrator}")]
    public class DocumentController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public Task<List<Document>> GetAll() => sender.Send(new GetAllDocumentsQuery());

        [HttpGet("{id}")] // Endpoint adress - in parenthesis additional route element to avoid targeting multiple endpoints at once
        public Task<Document?> GetById(Guid id) => sender.Send(new GetDocumentQuery(id));

        [HttpPost]
        public Task<Guid> Add(CreateDocumentCommand command) => sender.Send(command);

        [HttpPut]
        public Task Update(UpdateDocumentCommand command) => sender.Send(command);

        [HttpDelete("{id}")]
        public Task Delete(Guid id) => sender.Send(new DeleteDocumentCommand(id));
    }
}
