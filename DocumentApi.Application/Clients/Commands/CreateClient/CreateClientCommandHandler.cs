using DocumentApi.Application.Common.Interfaces;
using DocumentApi.Domain.Entities;
using MediatR;

namespace DocumentApi.Application.Clients.Commands.CreateClient
{
    public class CreateClientCommandHandler(IDocumentDbContext context) : IRequestHandler<CreateClientCommand, int>
    {
        public async Task<int> Handle(CreateClientCommand command, CancellationToken cancellationToken)
        {
            var newClientEntity = new Client()
            {
                Name = command.Name,
                Email = command.Email,
                TelephoneNumber = command.TelephoneNumber
            };

            context.Clients.Add(newClientEntity);

            await context.SaveChangesAsync(cancellationToken);

            return newClientEntity.Id;
        }
    }
}
