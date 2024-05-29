using ClientApplication.Classes;
using DocumentApi.Domain.Entities;

namespace ClientApplication.Interfaces
{
    public interface IApiTranslatorService
    {
        public Task<ApiResponse<List<Translator>>> GetAll();

        public Task<ApiResponse<Translator>> GetById(int id);

        public Task<ApiResponse<Translator>> Create(Translator translator);

        public Task<ApiResponse<Translator>> Update(Translator translator);

        public Task<ApiResponse<Translator>> Delete(int id);
    }
}
