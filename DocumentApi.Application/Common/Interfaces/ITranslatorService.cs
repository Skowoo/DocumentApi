using DocumentApi.Domain.Entities;

namespace DocumentApi.Application.Common.Interfaces
{
    public interface ITranslatorService
    {
        public List<Translator> GetAll();

        public Translator? GetById(int id);

        public void Add(Translator translator);

        public void Update(Translator translator);

        public void Delete(int id);
    }
}
