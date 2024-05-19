using DocumentApi.Application.Common.Interfaces;
using MediatR;

namespace DocumentApi.Application.Documents.Commands.UpdateDocument
{
    public class UpdateDocumentCommandHandler(IDocumentDbContext context) : IRequestHandler<UpdateDocumentCommand>
    {
        public async Task Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
        {
            var targetEntity = await context.Documents.FindAsync([request.Id], cancellationToken);

            if (targetEntity is not null)
            {
                targetEntity.Title = request.Title;
                targetEntity.SignsSize = request.SignsSize;
                targetEntity.CreatedAt = request.CreatedAt;
                targetEntity.Deadline = request.Deadline;
                targetEntity.ClientId = request.ClientId;
                targetEntity.TranslatorId = request.TranslatorId;

                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
