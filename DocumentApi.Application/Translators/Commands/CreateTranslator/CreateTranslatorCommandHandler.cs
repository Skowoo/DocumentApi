using DocumentApi.Application.Common.Interfaces;
using DocumentApi.Domain.Entities;
using MediatR;

namespace DocumentApi.Application.Translators.Commands.CreateTranslator
{
    public class CreateTranslatorCommandHandler(IDocumentDbContext context) : IRequestHandler<CreateTranslatorCommand, int>
    {
        public async Task<int> Handle(CreateTranslatorCommand request, CancellationToken cancellationToken)
        {
            var newEntity = new Translator
            {
                Name = request.Name,
            };

            context.Translators.Add(newEntity);

            await context.SaveChangesAsync(cancellationToken);

            return newEntity.Id;
        }
    }
}
