using ClientApplication.Classes;
using DocumentApi.Domain.Entities;

namespace ClientApplication.Interfaces
{
    public interface IApiDocumentService
    {
        Task<ApiResponse<List<Document>>> GetAllAsync();

        Task<ApiResponse<Document>> GetByIdAsync(string id);

        Task<ApiResponse<Document>> CreateAsync(Document translator);

        Task<ApiResponse<Document>> UpdateAsync(Document translator);

        Task<ApiResponse<Document>> DeleteAsync(string id);
    }
}
