using MediatR;

namespace DocumentApi.Application.Documents.Commands.DeleteDocument
{
    public record DeleteDocumentCommand(Guid Id) : IRequest;
}
