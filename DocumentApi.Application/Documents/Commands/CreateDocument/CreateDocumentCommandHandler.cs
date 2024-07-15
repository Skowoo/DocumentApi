using DocumentApi.Application.Common.Interfaces;
using DocumentApi.Domain.Entities;
using MediatR;

namespace DocumentApi.Application.Documents.Commands.CreateDocument
{
    public class CreateDocumentCommandHandler(IDocumentDbContext context, ITimeProvider timeProvider) : IRequestHandler<CreateDocumentCommand, Guid>
    {
        public async Task<Guid> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        {
            var newEntity = new Document
            {
                Title = request.Title,
                SignsSize = request.SignsSize,
                CreatedAt = timeProvider.GetCurrentTime(),
                Deadline = request.Deadline,
                ClientId = request.ClientId,                
                Client = await context.Clients.FindAsync([request.ClientId], cancellationToken),
                TranslatorId = request.TranslatorId,
                Translator = await context.Translators.FindAsync([request.TranslatorId], cancellationToken)
            };

            context.Documents.Add(newEntity);

            await context.SaveChangesAsync(cancellationToken);

            return newEntity.Id;
        }
    }
}
