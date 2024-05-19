using DocumentApi.Application.Common.Interfaces;

namespace DocumentApi.Application.Clients.Commands.CreateClient
{
    public record CreateClientCommand : ICommand<int>
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? TelephoneNumber { get; set; }
    }
}
