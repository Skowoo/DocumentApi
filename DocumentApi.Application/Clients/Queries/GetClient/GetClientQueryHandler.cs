using DocumentApi.Application.Common.Interfaces;
using DocumentApi.Domain.Entities;
using MediatR;

namespace DocumentApi.Application.Clients.Queries.GetClient
{
    public class GetClientQueryHandler(IDocumentDbContext context) : IRequestHandler<GetClientQuery, Client?>
    {
        public async Task<Client?> Handle(GetClientQuery request, CancellationToken cancellationToken)
            => await context.Clients.FindAsync([request.Id], cancellationToken);
    }
}
