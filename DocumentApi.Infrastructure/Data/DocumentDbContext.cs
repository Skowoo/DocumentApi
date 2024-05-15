using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DocumentApi.Infrastructure.Data
{
    public class DocumentDbContext(DbContextOptions<DocumentDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Document> Documents { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Translator> Translators { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Client>().HasData(
                new Client { Id = 1, Email = "pierwszyKlient@poczta.pl", Name = "Januszex", TelephoneNumber = "168-483-241"},
                new Client { Id = 2, Email = "drugiKlient@poczta.pl", Name = "MarPol", TelephoneNumber = "268-483-242" },
                new Client { Id = 3, Email = "trzeciKlient@poczta.pl", Name = "Januszex", TelephoneNumber = "368-483-243" }
                );

            builder.Entity<Translator>().HasData(
                new Translator { Id = 1, Name = "Joanna"},
                new Translator { Id = 2, Name = "Paweł" },
                new Translator { Id = 3, Name = "Maria" }
                );

            builder.Entity<Document>().HasData(
                new Document { Id = Guid.NewGuid(), ClientId = 1, CreatedAt = DateTime.Now, Deadline = DateTime.Now.AddDays(1), SignsSize = 144, Title = "Dokument1", TranslatorId = null },
                new Document { Id = Guid.NewGuid(), ClientId = 2, CreatedAt = DateTime.Now, Deadline = DateTime.Now.AddDays(1), SignsSize = 144, Title = "Dokument1", TranslatorId = 1 },
                new Document { Id = Guid.NewGuid(), ClientId = 2, CreatedAt = DateTime.Now, Deadline = DateTime.Now.AddDays(1), SignsSize = 144, Title = "Dokument1", TranslatorId = 2 },
                new Document { Id = Guid.NewGuid(), ClientId = 2, CreatedAt = DateTime.Now, Deadline = DateTime.Now.AddDays(1), SignsSize = 144, Title = "Dokument1", TranslatorId = 1 },
                new Document { Id = Guid.NewGuid(), ClientId = 2, CreatedAt = DateTime.Now, Deadline = DateTime.Now.AddDays(1), SignsSize = 144, Title = "Dokument1", TranslatorId = null }
                );

            // Need to be called in order to construct Identity tables!!!
            base.OnModelCreating(builder);
        }
    }
}
