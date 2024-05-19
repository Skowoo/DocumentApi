using MediatR;

namespace DocumentApi.Application.Translators.Commands.UpdateTranslator
{
    public record UpdateTranslatorCommand : IRequest
    {
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}
