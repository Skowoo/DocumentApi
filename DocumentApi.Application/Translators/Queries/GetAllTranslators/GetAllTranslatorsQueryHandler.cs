using DocumentApi.Application.Common.Interfaces;
using DocumentApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentApi.Application.Translators.Queries.GetAllTranslators
{
    public class GetAllTranslatorsQueryHandler(IDocumentDbContext context) : IRequestHandler<GetAllTranslatorsQuery, List<Translator>>
    {
        public async Task<List<Translator>> Handle(GetAllTranslatorsQuery request, CancellationToken cancellationToken)
            => await context.Translators.ToListAsync(cancellationToken);
    }
}
