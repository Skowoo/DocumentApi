using DocumentApi.Application.Common.Interfaces;

namespace DocumentApi.Application.Translators.Commands.CreateTranslator
{
    public record CreateTranslatorCommand : ICommand<int>
    {
        public string? Name { get; set; }
    }
}
