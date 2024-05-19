using DocumentApi.Application.Common.Interfaces;
using DocumentApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DocumentApi.Application.Clients.Queries.GetAllClients
{
    public class GetAllClientsQueryHandler(IDocumentDbContext context) : IRequestHandler<GetAllClientsQuery, List<Client>>
    {
        public async Task<List<Client>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken) 
            => await context.Clients.ToListAsync(cancellationToken);
    }
}
