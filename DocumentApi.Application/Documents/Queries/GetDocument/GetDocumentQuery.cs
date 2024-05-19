using DocumentApi.Domain.Entities;
using MediatR;

namespace DocumentApi.Application.Documents.Queries.GetDocument
{
    public record GetDocumentQuery(Guid Id) : IRequest<Document?>;
}
