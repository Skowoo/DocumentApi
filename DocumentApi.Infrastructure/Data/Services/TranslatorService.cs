using DocumentApi.Application.Interfaces;
using DocumentApi.Domain.Entities;

namespace DocumentApi.Infrastructure.Data.Services
{
    public class TranslatorService(DocumentDbContext context) : ITranslatorService
    {
        public void Add(Translator translator)
        {
            context.Translators.Add(translator);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var target = context.Translators.FirstOrDefault(x => x.Id == id);
            if (target is not null)
                context.Translators.Remove(target);

            context.SaveChanges();
        }

        public List<Translator> GetAll() => [.. context.Translators];

        public Translator? GetById(int id) => context.Translators.SingleOrDefault(x => x.Id == id);

        public void Update(Translator translator)
        {
            var target = context.Translators.FirstOrDefault(x => x.Id == translator.Id);
            if (target is not null)
                context.Translators.Update(target);

            context.SaveChanges();
        }
    }
}
