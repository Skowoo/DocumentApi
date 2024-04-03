using DocumentApi.Domain.Entities;

namespace DocumentApi.Application.Interfaces
{
    public interface IDocumentService
    {
        public List<Document> GetAll();

        public Document? GetById(Guid id);
    }
}
