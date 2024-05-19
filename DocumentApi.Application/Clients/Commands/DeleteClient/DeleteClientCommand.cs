using DocumentApi.Application.Common.Interfaces;

namespace DocumentApi.Application.Clients.Commands.DeleteClient
{
    public record DeleteClientCommand(int Id) : ICommand;
}
