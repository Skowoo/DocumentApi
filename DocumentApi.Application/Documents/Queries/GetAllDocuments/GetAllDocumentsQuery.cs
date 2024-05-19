using DocumentApi.Domain.Entities;
using MediatR;

namespace DocumentApi.Application.Documents.Queries.GetAllDocuments
{
    public record GetAllDocumentsQuery : IRequest<List<Document>>;
}
