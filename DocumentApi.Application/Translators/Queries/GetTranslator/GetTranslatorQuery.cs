using DocumentApi.Domain.Entities;
using MediatR;

namespace DocumentApi.Application.Translators.Queries.GetTranslator
{
    public record GetTranslatorQuery(int Id) : IRequest<Translator?>;
}
