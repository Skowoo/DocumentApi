using DocumentApi.Application.Common.Interfaces;

namespace DocumentApi.Application.Translators.Commands.DeleteTranslator
{
    public record DeleteTranslatorCommand(int Id) : ICommand;
}
