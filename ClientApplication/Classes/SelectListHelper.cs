using ClientApplication.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClientApplication.Classes
{
    public class SelectListHelper(
        IRestService<Translator> translatorService,
        IRestService<Client> clientService)
    {
        static readonly string DefaultSelectText = "Wybierz...";

        public async Task<SelectList> GetTranslatorsSelectListAsync()
        {
            var translatorsResult = await translatorService.GetAllAsync();

            var selectListData = 
                new List<Translator> { new() { Name = DefaultSelectText } }
                .Union([.. translatorsResult.Data!.OrderBy(x => x.Name)]);

            return new SelectList(selectListData, nameof(Translator.Id), nameof(Translator.Name));
        }

        public async Task<SelectList> GetClientsSelectListAsync()
        {         
            var clientsResult = await clientService.GetAllAsync();

            var selectListData = 
                new List<Client> { new() { Name = DefaultSelectText } }
                .Union([.. clientsResult.Data!.OrderBy(x => x.Name)]);

            return new SelectList(selectListData, nameof(Client.Id), nameof(Client.Name));
        }
    }
}
