using DocumentApi.Application.Common.Interfaces;
using DocumentApi.Domain.Entities;
using MediatR;

namespace DocumentApi.Application.Documents.Queries.GetDocument
{
    public class GetDocumentQueryHandler(IDocumentDbContext context) : IRequestHandler<GetDocumentQuery, Document?>
    {
        public async Task<Document?> Handle(GetDocumentQuery request, CancellationToken cancellationToken) 
            => await context.Documents.FindAsync([request.Id], cancellationToken);
    }
}
