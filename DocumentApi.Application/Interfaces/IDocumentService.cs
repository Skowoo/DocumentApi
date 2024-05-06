using DocumentApi.Domain.Entities;

namespace DocumentApi.Application.Interfaces
{
    public interface IDocumentService
    {
        public List<Document> GetAll();

        public Document? GetById(Guid id);

        public void Add(Document document);

        public void Update(Document document);

        public void Delete(Document document);
    }
}
