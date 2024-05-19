using DocumentApi.Domain.Entities;
using MediatR;

namespace DocumentApi.Application.Clients.Queries.GetClient
{
    public record GetClientQuery(int Id) : IRequest<Client?>;
}
