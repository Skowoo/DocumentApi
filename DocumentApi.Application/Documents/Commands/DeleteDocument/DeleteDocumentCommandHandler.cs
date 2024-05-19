using DocumentApi.Application.Common.Interfaces;
using MediatR;

namespace DocumentApi.Application.Documents.Commands.DeleteDocument
{
    public class DeleteDocumentCommandHandler(IDocumentDbContext context) : IRequestHandler<DeleteDocumentCommand>
    {
        public async Task Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var targetEntity = await context.Documents.FindAsync([request.Id], cancellationToken);

            if (targetEntity is not null)
            {
                context.Documents.Remove(targetEntity);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
