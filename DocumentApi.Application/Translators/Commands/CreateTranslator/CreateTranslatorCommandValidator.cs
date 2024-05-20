using FluentValidation;

namespace DocumentApi.Application.Translators.Commands.CreateTranslator
{
    public class CreateTranslatorCommandValidator : AbstractValidator<CreateTranslatorCommand>
    {
        public CreateTranslatorCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }
}
