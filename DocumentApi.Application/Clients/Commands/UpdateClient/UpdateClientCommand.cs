using DocumentApi.Application.Common.Interfaces;

namespace DocumentApi.Application.Clients.Commands.UpdateClient
{
    public record UpdateClientCommand : ICommand
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? TelephoneNumber { get; set; }
    }
}
