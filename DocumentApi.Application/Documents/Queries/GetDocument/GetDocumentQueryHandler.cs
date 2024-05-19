using DocumentApi.Application.Common.Interfaces;
using DocumentApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentApi.Application.Documents.Queries.GetDocument
{
    public class GetDocumentQueryHandler(IDocumentDbContext context) : IRequestHandler<GetDocumentQuery, Document?>
    {
        public async Task<Document?> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
            => await context.Documents
                .Include(x => x.Translator)
                .Include(x => x.Client)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
    }
}
