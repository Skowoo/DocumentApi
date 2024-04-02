using DocumentApi.Infrastructure.Data.MemoryService;

namespace Infrastructure.UnitTests
{
    [TestClass]
    public class MemoryServiceTests
    {
        [TestMethod]
        public void ServiceInitialization()
        {
            _ = new DocumentMemoryService();
            Assert.AreEqual(5, DocumentMemoryService.Clients.Count);
            Assert.AreEqual(5, DocumentMemoryService.Translators.Count);
            Assert.AreEqual(100, DocumentMemoryService.Documents.Count);

            _ = new DocumentMemoryService();
            Assert.AreEqual(5, DocumentMemoryService.Clients.Count);
            Assert.AreEqual(5, DocumentMemoryService.Translators.Count);
            Assert.AreEqual(100, DocumentMemoryService.Documents.Count);
        }

        [TestMethod]
        public void AddClient_OKTest()
        {
            _ = new DocumentMemoryService();
            Assert.AreEqual(5, DocumentMemoryService.Clients.Count);
            Assert.IsNull(DocumentMemoryService.Clients.Where(x => x.Id == 900).SingleOrDefault());
            DocumentMemoryService.Clients.Add(new DocumentApi.Domain.Entities.Client() { Id = 900 });
            Assert.AreEqual(6, DocumentMemoryService.Clients.Count);
            Assert.IsNotNull(DocumentMemoryService.Clients.Where(x => x.Id == 900).SingleOrDefault());
        }

        [TestMethod]
        public void AddTranslator_OKTest()
        {
            _ = new DocumentMemoryService();
            Assert.AreEqual(5, DocumentMemoryService.Translators.Count);
            Assert.IsNull(DocumentMemoryService.Translators.Where(x => x.Id == 900).SingleOrDefault());
            DocumentMemoryService.Translators.Add(new DocumentApi.Domain.Entities.Translator() { Id = 900 });
            Assert.AreEqual(6, DocumentMemoryService.Translators.Count);
            Assert.IsNotNull(DocumentMemoryService.Translators.Where(x => x.Id == 900).SingleOrDefault());
        }

        [TestMethod]
        public void AddDocument_OKTest()
        {
            _ = new DocumentMemoryService();
            Guid exampleId = new();
            Assert.AreEqual(100, DocumentMemoryService.Documents.Count);
            Assert.IsNull(DocumentMemoryService.Documents.Where(x => x.Id == exampleId).SingleOrDefault());
            DocumentMemoryService.Documents.Add(new DocumentApi.Domain.Entities.Document() { Id = exampleId });
            Assert.AreEqual(101, DocumentMemoryService.Documents.Count);
            Assert.IsNotNull(DocumentMemoryService.Documents.Where(x => x.Id == exampleId).SingleOrDefault());
        }
    }
}