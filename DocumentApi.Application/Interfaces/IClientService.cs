using DocumentApi.Domain.Entities;

namespace DocumentApi.Application.Interfaces
{
    public interface IClientService
    {
        public List<Client> GetAll();

        public Client? GetById (int id);

        public void Add(Client document);

        public void Update(Client document);

        public void Delete(Client document);
    }
}
