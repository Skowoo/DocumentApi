using DocumentApi.Application.Common.Interfaces;
using DocumentApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentApi.Application.Documents.Queries.GetAllDocuments
{
    public class GetAllDocumentsQueryHandler(IDocumentDbContext context) : IRequestHandler<GetAllDocumentsQuery, List<Document>>
    {
        public async Task<List<Document>> Handle(GetAllDocumentsQuery request, CancellationToken cancellationToken)
            => await context.Documents
                .Include(x => x.Translator)
                .Include(x => x.Client)
                .ToListAsync(cancellationToken);
    }
}
