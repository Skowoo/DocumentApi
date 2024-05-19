using DocumentApi.Domain.Entities;
using MediatR;

namespace DocumentApi.Application.Translators.Queries.GetAllTranslators
{
    public record GetAllTranslatorsQuery : IRequest<List<Translator>>;
}
