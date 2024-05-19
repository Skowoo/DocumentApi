using DocumentApi.Domain.Entities;
using MediatR;

namespace DocumentApi.Application.Clients.Queries.GetAllClients
{
    public record GetAllClientsQuery : IRequest<List<Client>>;
}
