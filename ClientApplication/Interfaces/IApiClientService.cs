using ClientApplication.Classes;
using DocumentApi.Domain.Entities;

namespace ClientApplication.Interfaces
{
    public interface IApiClientService
    {
        Task<ApiResponse<List<Client>>> GetAll();

        Task<ApiResponse<Client>> GetById(int id);

        Task<ApiResponse<Client>> Create(Client translator);

        Task<ApiResponse<Client>> Update(Client translator);

        Task<ApiResponse<Client>> Delete(int id);
    }
}
