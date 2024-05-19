using DocumentApi.Application.Common.Interfaces;
using DocumentApi.Domain.Entities;
using MediatR;

namespace DocumentApi.Application.Documents.Commands.CreateDocument
{
    public class CreateDocumentCommandHandler(IDocumentDbContext context) : IRequestHandler<CreateDocumentCommand, Guid>
    {
        public async Task<Guid> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        {
            var newEntity = new Document
            {
                Title = request.Title,
                SignsSize = request.SignsSize,
                CreatedAt = request.CreatedAt,
                Deadline = request.Deadline,
                ClientId = request.ClientId,
                TranslatorId = request.TranslatorId,
            };

            context.Documents.Add(newEntity);

            await context.SaveChangesAsync(cancellationToken);

            return newEntity.Id;
        }
    }
}
