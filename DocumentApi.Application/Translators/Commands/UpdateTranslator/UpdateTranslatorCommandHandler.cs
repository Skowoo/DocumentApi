using DocumentApi.Application.Common.Interfaces;
using MediatR;

namespace DocumentApi.Application.Translators.Commands.UpdateTranslator
{
    public class UpdateTranslatorCommandHandler(IDocumentDbContext context) : IRequestHandler<UpdateTranslatorCommand>
    {
        public async Task Handle(UpdateTranslatorCommand request, CancellationToken cancellationToken)
        {
            var targetEntity = await context.Translators.FindAsync([request.Id], cancellationToken);

            if (targetEntity is not null)
            {
                targetEntity.Name = request.Name;
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
