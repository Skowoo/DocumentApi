namespace DocumentApi.Domain.Entities
{
    public class Document
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public int SignsSize { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime Deadline { get; set; }

        public Client? Client { get; set; }
        public int ClientId { get; set; }

        public Translator? Translator { get; set; }
        public int? TranslatorId { get; set; }
    }
}
