using FluentValidation;

namespace DocumentApi.Application.Translators.Commands.UpdateTranslator
{
    public class UpdateTranslatorCommandValidator : AbstractValidator<UpdateTranslatorCommand>
    {
        public UpdateTranslatorCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }
}
