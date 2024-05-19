using DocumentApi.Application.Common.Interfaces;

namespace DocumentApi.Application.Documents.Commands.DeleteDocument
{
    public record DeleteDocumentCommand(Guid Id) : ICommand;
}
