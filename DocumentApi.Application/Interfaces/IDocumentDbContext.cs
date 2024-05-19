using DocumentApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocumentApi.Application.Interfaces
{
    public interface IDocumentDbContext
    {
        DbSet<Client> Clients { get; }

        DbSet<Document> Documents { get; }

        DbSet<Translator> Translators { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
