using MediatR;

namespace DocumentApi.Application.Clients.Commands.CreateClient
{
    public record CreateClientCommand : IRequest<int>
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? TelephoneNumber { get; set; }
    }
}
