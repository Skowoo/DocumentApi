using MediatR;

namespace DocumentApi.Application.Clients.Commands.UpdateClient
{
    public record UpdateClientCommand : IRequest
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? TelephoneNumber { get; set; }
    }
}
