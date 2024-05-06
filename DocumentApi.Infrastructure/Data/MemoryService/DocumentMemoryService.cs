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

        void IDocumentService.Add(Document document) => Documents.Add(document);

        void IDocumentService.Update(Document document)
        {
            Document? target = Documents.SingleOrDefault(x => x.Id == document.Id) 
                ?? throw new FileNotFoundException("Document not found!");

            CopyAllFields(document, target);
        }

        void IDocumentService.Delete(Document document)
        {
            Document? target = Documents.SingleOrDefault(x => x.Id == document.Id)
                ?? throw new FileNotFoundException("Document not found!");

            Documents.Remove(target);
        }

        #endregion

        #region Clients section

        List<Client> IClientService.GetAll() => Clients;

        Client? IClientService.GetById(int id) => Clients.Where(x => x.Id == id).SingleOrDefault();

        void IClientService.Add(Client client) => Clients.Add(client);

        void IClientService.Update(Client client)
        {
            Client? target = Clients.FirstOrDefault(x => x.Id == client.Id) 
                ?? throw new FileNotFoundException("Client not found!");

            CopyAllFields(client, target);
        }

        void IClientService.Delete(Client client)
        {
            Client? target = Clients.SingleOrDefault(x => x.Id == client.Id) 
                ?? throw new FileNotFoundException("Client not found!");

            Clients.Remove(target);
        }

        #endregion

        #region Translators section

        List<Translator> ITranslatorService.GetAll() => Translators;

        Translator? ITranslatorService.GetById(int id) => Translators.Where(x => x.Id == id).SingleOrDefault();

        public void Add(Translator translator) => Translators.Add(translator);

        public void Update(Translator translator)
        {
            Translator? target = Translators.SingleOrDefault(x => x.Id == translator.Id) 
                ?? throw new FileNotFoundException("Translator not found!");

            CopyAllFields(translator, target);
        }

        public void Delete(Translator translator)
        {
            Translator? target = Translators.SingleOrDefault(x => x.Id == translator.Id)
                ?? throw new FileNotFoundException("Translator not found!");

            Translators.Remove(target);
        }

        #endregion

        private static void CopyAllFields(object source, object target)
        {
            if (target.GetType() != source.GetType())
                throw new ArgumentException("Given objects have different type!");

            foreach (var prop in source.GetType().GetProperties())
                prop.SetValue(target, prop.GetValue(source));
        }
    }
}
