using DocumentApi.Domain.Entities;

namespace DocumentApi.Application.Interfaces
{
    public interface ITranslatorService
    {
        public List<Translator> GetAll();

        public Translator? GetById(int id);

        public void Add(Translator document);

        public void Update(Translator document);

        public void Delete(Translator document);
    }
}
