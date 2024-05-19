using DocumentApi.Application.Common.Interfaces;
using MediatR;

namespace DocumentApi.Application.Clients.Commands.DeleteClient
{
    public class DeleteClientCommandHandler(IDocumentDbContext context) : IRequestHandler<DeleteClientCommand>
    {
        public async Task Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            var targetEntity = await context.Clients.FindAsync([request.Id], cancellationToken);

            if (targetEntity is not null)
            {
                context.Clients.Remove(targetEntity);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
