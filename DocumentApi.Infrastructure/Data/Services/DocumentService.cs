using DocumentApi.Application.Interfaces;
using DocumentApi.Domain.Entities;

namespace DocumentApi.Infrastructure.Data.Services
{
    internal class DocumentService(DocumentDbContext context) : IDocumentService
    {
        public void Add(Document document)
        {
            context.Documents.Add(document);
            context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var target = context.Documents.SingleOrDefault(d => d.Id == id);
            if (target is not null)
                context.Documents.Remove(target);

            context.SaveChanges();
        }

        public List<Document> GetAll() => [.. context.Documents];

        public Document? GetById(Guid id) => context.Documents.SingleOrDefault(x => x.Id == id);

        public void Update(Document document)
        {
            var target = context.Documents.SingleOrDefault(d => d.Id == document.Id);
            if (target is not null)
                context.Documents.Update(target);

            context.SaveChanges();
        }
    }
}
