using ClientApplication.Classes;

namespace ClientApplication.Interfaces
{
    public interface IRestService<TItem> where TItem : class
    {
        Task<ApiResponse<List<TItem>>> GetAllAsync();

        Task<ApiResponse<TItem>> GetByIdAsync(int id);

        Task<ApiResponse<TItem>> GetByIdAsync(string id);

        Task<ApiResponse<TItem>> CreateAsync(TItem input);

        Task<ApiResponse<TItem>> UpdateAsync(TItem input);

        Task<ApiResponse<TItem>> DeleteAsync(int id);

        Task<ApiResponse<TItem>> DeleteAsync(string id);
    }
}