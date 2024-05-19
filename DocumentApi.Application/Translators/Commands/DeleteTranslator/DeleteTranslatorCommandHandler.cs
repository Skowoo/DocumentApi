using DocumentApi.Application.Common.Interfaces;
using MediatR;

namespace DocumentApi.Application.Translators.Commands.DeleteTranslator
{
    public class DeleteTranslatorCommandHandler(IDocumentDbContext context) : IRequestHandler<DeleteTranslatorCommand>
    {
        public async Task Handle(DeleteTranslatorCommand request, CancellationToken cancellationToken)
        {
            var targetEntity = await context.Translators.FindAsync([request.Id], cancellationToken);

            if (targetEntity is not null)
            {
                context.Translators.Remove(targetEntity);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
