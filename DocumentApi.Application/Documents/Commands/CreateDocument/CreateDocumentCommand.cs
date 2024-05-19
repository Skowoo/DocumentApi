using MediatR;

namespace DocumentApi.Application.Documents.Commands.CreateDocument
{
    public record CreateDocumentCommand : IRequest<Guid>
    {
        public string? Title { get; set; }

        public int SignsSize { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime Deadline { get; set; }

        public int ClientId { get; set; }

        public int? TranslatorId { get; set; }
    }
}
