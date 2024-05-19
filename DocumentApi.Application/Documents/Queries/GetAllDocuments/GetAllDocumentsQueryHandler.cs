using DocumentApi.Application.Common.Interfaces;
using DocumentApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentApi.Application.Documents.Queries.GetAllDocuments
{
    public class GetAllDocumentsQueryHandler(IDocumentDbContext context) : IRequestHandler<GetAllDocumentsQuery, List<Document>>
    {
        public Task<List<Document>> Handle(GetAllDocumentsQuery request, CancellationToken cancellationToken)
            => context.Documents.ToListAsync(cancellationToken);
    }
}
