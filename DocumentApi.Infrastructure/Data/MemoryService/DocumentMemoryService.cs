using DocumentApi.Application.Interfaces;
using DocumentApi.Domain.Entities;

namespace DocumentApi.Infrastructure.Data.MemoryService
{
    public class DocumentMemoryService : IDocumentService, IClientService, ITranslatorService
    {
        public readonly static List<Document> Documents = [];

        public readonly static List<Client> Clients = [];

        public readonly static List<Translator> Translators = [];

        public DocumentMemoryService()
        {
            ClearData();
            FillExampleList();
        }

        static void ClearData()
        {
            Documents.Clear();
            Clients.Clear();
            Translators.Clear();
        }

        static void FillExampleList(int clientsCount = 5, int translatorsCount = 5, int documentCount = 100)
        {
            Random rnd = new();

            for (int i = 1; i <= clientsCount; i++)
                Clients.Add(
                    new Client()
                    {
                        Id = i,
                        Name = $"Klient{i}",
                        Email = $"mail{i}@poczta.pl",
                        TelephoneNumber = rnd.Next(100000000, 999999999).ToString()
                    });

            for (int i = 1; i <= translatorsCount; i++)
                Translators.Add(new Translator()
                {
                    Id = i,
                    Name = $"Tłumacz{i}"
                });

            for (int i =  1; i <= documentCount; i++)
            {
                var translator = rnd.Next(0, (int)(1.5D * clientsCount)) > clientsCount ? null : Translators[rnd.Next(0, clientsCount)];
                Documents.Add(
                    new Document()
                    {
                        Id = Guid.NewGuid(),
                        Title = $"Test{i}",
                        SignsSize = rnd.Next(100, 1000),
                        CreatedAt = DateTime.Now,
                        Deadline = DateTime.Now.AddDays(rnd.Next(-5, 30)),
                        Client = Clients[rnd.Next(0, clientsCount)],
                        Translator = translator
                    });
            }
        }

        #region Documents section

        List<Document> IDocumentService.GetAll() => Documents;

        Document? IDocumentService.GetById(Guid id) => Documents.Where(x => x.Id == id).SingleOrDefault();

        #endregion

        #region Clients section

        List<Client> IClientService.GetAll() => Clients;

        Client? IClientService.GetById(int id) => Clients.Where(x => x.Id == id).SingleOrDefault();

        #endregion

        #region Translators section

        List<Translator> ITranslatorService.GetAll() => Translators;

        Translator? ITranslatorService.GetById(int id) => Translators.Where(x => x.Id == id).SingleOrDefault();

        #endregion
    }
}
