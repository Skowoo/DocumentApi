using DocumentApi.Application.Common.Interfaces;

namespace DocumentApi.Application.Translators.Commands.UpdateTranslator
{
    public record UpdateTranslatorCommand : ICommand
    {
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}
