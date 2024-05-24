using DocumentApi.Application.Common.Interfaces;
using FluentValidation;

namespace DocumentApi.Application.Documents.Commands.UpdateDocument
{
    public class UpdateDocumentCommandValidator : AbstractValidator<UpdateDocumentCommand>
    {
        public UpdateDocumentCommandValidator(IDocumentDbContext context)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(documentId => context.Documents.Any(document => document.Id == documentId))
                .WithMessage("Document with given Id does not exist in database");

            RuleFor(x => x.Title)
                .NotEmpty();

            RuleFor(x => x.SignsSize)
                .NotEmpty()
                .InclusiveBetween(1, 100_000);

            RuleFor(x => x.CreatedAt)
                .NotEmpty();

            RuleFor(x => x.Deadline)
                .NotEmpty();

            RuleFor(x => new { x.CreatedAt, x.Deadline })
                .Must(x => x.CreatedAt <= x.Deadline)
                .WithMessage("Deadline must be after the creation date!");

            RuleFor(x => x.ClientId)
                .NotEmpty()
                .Must(clientId => context.Clients.Any(client => client.Id == clientId))
                .WithMessage("Client with given Id does not exist in database");

            When(x => x.TranslatorId.HasValue, () =>
            {
                RuleFor(x => x.TranslatorId)
                    .Must(translatorId => context.Translators.Any(translator => translator.Id == translatorId))
                    .WithMessage("Translator with given Id does not exist in database");
            });
        }
    }
}
