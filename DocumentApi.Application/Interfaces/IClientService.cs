using DocumentApi.Domain.Entities;

namespace DocumentApi.Application.Interfaces
{
    public interface IClientService
    {
        public List<Client> GetAll();

        public Client? GetById (int id);

        public void Add(Client client);

        public void Update(Client client);

        public void Delete(int id);
    }
}
