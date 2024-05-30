using ClientApplication.Classes;
using DocumentApi.Domain.Entities;

namespace ClientApplication.Interfaces
{
    public interface IApiDocumentService
    {
        Task<ApiResponse<List<Document>>> GetAll();

        Task<ApiResponse<Document>> GetById(string id);

        Task<ApiResponse<Document>> Create(Document translator);

        Task<ApiResponse<Document>> Update(Document translator);

        Task<ApiResponse<Document>> Delete(string id);
    }
}
