using MediatR;

namespace DocumentApi.Application.Translators.Commands.CreateTranslator
{
    public record CreateTranslatorCommand : IRequest<int>
    {
        public string? Name { get; set; }
    }
}
