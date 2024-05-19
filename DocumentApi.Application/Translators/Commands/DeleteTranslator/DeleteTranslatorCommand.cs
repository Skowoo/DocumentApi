using MediatR;

namespace DocumentApi.Application.Translators.Commands.DeleteTranslator
{
    public record DeleteTranslatorCommand(int Id) : IRequest;
}
