using MediatR;

namespace DocumentApi.Application.Clients.Commands.DeleteClient
{
    public record DeleteClientCommand(int Id) : IRequest;
}
