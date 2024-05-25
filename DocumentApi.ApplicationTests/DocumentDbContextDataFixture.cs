using DocumentApi.Application.Common.Interfaces;
using DocumentApi.Domain.Entities;
using DocumentApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentApi.ApplicationTests
{
    internal static class DocumentDbContextDataFixture
    {
        public static IDocumentDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<DocumentDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var context = new DocumentDbContext(options);

            context.Clients.Add(new Client { Id = 1, Name = "Klient1" });
            context.Translators.Add(new Translator { Id = 1, Name = "Tłumacz1" });
            context.Documents.Add(new Document { Id = Guid.Parse("01cbe5e5-ae75-4972-b741-14bf69f33f48") });
            context.SaveChanges();

            return context;
        }
    }
}
