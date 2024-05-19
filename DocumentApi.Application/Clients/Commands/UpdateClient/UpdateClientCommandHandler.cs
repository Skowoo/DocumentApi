using DocumentApi.Application.Interfaces;
using MediatR;

namespace DocumentApi.Application.Clients.Commands.UpdateClient
{
    public class UpdateClientCommandHandler(IDocumentDbContext context) : IRequestHandler<UpdateClientCommand>
    {
        public async Task Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var targetEntity = await context.Clients.FindAsync([request.Id], cancellationToken);

            if (targetEntity is not null)
            {
                targetEntity.TelephoneNumber = request.TelephoneNumber;
                targetEntity.Email = request.Email;
                targetEntity.Name = request.Name;

                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
