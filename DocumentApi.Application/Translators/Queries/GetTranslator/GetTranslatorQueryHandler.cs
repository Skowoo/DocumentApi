using DocumentApi.Application.Common.Interfaces;
using DocumentApi.Domain.Entities;
using MediatR;

namespace DocumentApi.Application.Translators.Queries.GetTranslator
{
    public class GetTranslatorQueryHandler(IDocumentDbContext context) : IRequestHandler<GetTranslatorQuery, Translator?>
    {
        public async Task<Translator?> Handle(GetTranslatorQuery request, CancellationToken cancellationToken)
            => await context.Translators.FindAsync([request.Id], cancellationToken);
    }
}
