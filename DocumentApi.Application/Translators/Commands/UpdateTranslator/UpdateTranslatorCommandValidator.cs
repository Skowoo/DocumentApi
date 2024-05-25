using DocumentApi.Application.Common.Interfaces;
using FluentValidation;

namespace DocumentApi.Application.Translators.Commands.UpdateTranslator
{
    public class UpdateTranslatorCommandValidator : AbstractValidator<UpdateTranslatorCommand>
    {
        public UpdateTranslatorCommandValidator(IDocumentDbContext context)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(id => context.Translators.Any(existingTranslator => existingTranslator.Id == id))
                .WithMessage("Translator with given Id does not exists in database!");

            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }
}
