using DocumentApi.Application.Interfaces;
using DocumentApi.Domain.Entities;

namespace DocumentApi.Infrastructure.Data.Services
{
    public class ClientService(DocumentDbContext context) : IClientService
    {
        public void Add(Client client)
        {
            context.Clients.Add(client);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var target = context.Clients.SingleOrDefault(x => x.Id == id);
            if (target is not null)
                context.Clients.Remove(target);

            context.SaveChanges();
        }

        public List<Client> GetAll() => [.. context.Clients];

        public Client? GetById(int id) => context.Clients.SingleOrDefault(x => x.Id == id);

        public void Update(Client client)
        {
            var target = context.Clients.SingleOrDefault(x => x.Id == client.Id);
            if (target is not null)
                context.Clients.Update(target);

            context.SaveChanges();
        }
    }
}
