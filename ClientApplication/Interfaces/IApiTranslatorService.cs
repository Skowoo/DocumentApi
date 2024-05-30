using ClientApplication.Classes;
using DocumentApi.Domain.Entities;

namespace ClientApplication.Interfaces
{
    public interface IApiTranslatorService
    {
        public Task<ApiResponse<List<Translator>>> GetAllAsync();

        public Task<ApiResponse<Translator>> GetByIdAsync(int id);

        public Task<ApiResponse<Translator>> CreateAsync(Translator translator);

        public Task<ApiResponse<Translator>> UpdateAsync(Translator translator);

        public Task<ApiResponse<Translator>> DeleteAsync(int id);
    }
}
