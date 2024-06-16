using DocumentApi.Application.Common.Interfaces;
using FluentValidation;

namespace DocumentApi.Application.Documents.Commands.CreateDocument
{
    public class CreateDocumentCommandValidator : AbstractValidator<CreateDocumentCommand>
    {
        public CreateDocumentCommandValidator(IDocumentDbContext context, ITimeProvider timeProvider)
        {
            RuleFor(x => x.Title)
                .NotEmpty();

            RuleFor(x => x.SignsSize)
                .NotEmpty()
                .InclusiveBetween(1, 100_000);

            RuleFor(x => x.Deadline)
                .NotEmpty();

            RuleFor(x => x.Deadline )
                .Must(x => timeProvider.GetCurrentTimeAsync().Result <= x)
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
