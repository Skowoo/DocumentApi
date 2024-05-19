using DocumentApi.Application.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DocumentApi.Infrastructure.Data
{
    public class DocumentDbContext(DbContextOptions<DocumentDbContext> options) : IdentityDbContext(options), IDocumentDbContext
    {
        public DbSet<Document> Documents { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Translator> Translators { get; set; }
    }
}
