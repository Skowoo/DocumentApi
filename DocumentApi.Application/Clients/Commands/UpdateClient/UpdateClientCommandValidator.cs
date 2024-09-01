using DocumentApi.Application.Common.Interfaces;
using FluentValidation;

namespace DocumentApi.Application.Clients.Commands.UpdateClient
{
    public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
    {
        public UpdateClientCommandValidator(IDocumentDbContext context)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(id => context.Clients.Any(existingClient => existingClient.Id == id))
                .WithMessage("Client with given Id does not exists in database!");

            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.TelephoneNumber)
                .NotEmpty()
                .Length(9, 12);
        }
    }
}
