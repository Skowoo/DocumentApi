using DocumentApi.Domain.Entities;
using MediatR;

namespace DocumentApi.Application.Documents.Commands.UpdateDocument
{
    public record UpdateDocumentCommand : IRequest
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public int SignsSize { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime Deadline { get; set; }

        public int ClientId { get; set; }

        public int? TranslatorId { get; set; }
    }
}
