using ClientApplication.Classes;
using DocumentApi.Domain.Entities;

namespace ClientApplication.Interfaces
{
    public interface IApiClientService
    {
        Task<ApiResponse<List<Client>>> GetAllAsync();

        Task<ApiResponse<Client>> GetByIdAsync(int id);

        Task<ApiResponse<Client>> CreateAsync(Client translator);

        Task<ApiResponse<Client>> UpdateAsync(Client translator);

        Task<ApiResponse<Client>> DeleteAsync(int id);
    }
}
