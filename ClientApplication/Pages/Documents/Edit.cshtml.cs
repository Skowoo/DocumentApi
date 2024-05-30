using DocumentApi.Domain.Constants;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RestSharp.Authenticators;
using RestSharp;
using ClientApplication.Config;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using ClientApplication.Interfaces;

namespace ClientApplication.Pages.Documents
{
    public class EditModel(CurrentUser user, IOptions<DocumentApiConfig> apiConfig, IApiTranslatorService translatorService, IApiClientService clientService) : PageModel
    {
        [BindProperty]
        public Document Document { get; set; } = default!;

        public SelectList ClientsList { get; set; } = default!;

        public SelectList TranslatorsList { get; set; } = default!;

        public async Task OnGetAsync(string id)
        {
            if (!user.IsInRole(Roles.User))
                RedirectToPage("/Index");

            var options = new RestClientOptions(apiConfig.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            var client = new RestClient(options);
            var request = new RestRequest($"Document/GetById/{id}");
            var response = client.ExecuteAsync(request, Method.Get).Result;
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                var downloadedEntity = JsonConvert.DeserializeObject<Document>(response.Content)!;
                if (downloadedEntity is null)
                    ModelState.TryAddModelError("Client", "Client not found");
                else
                    Document = downloadedEntity;
            }

            var translatorsResult = await translatorService.GetAll();
            TranslatorsList = new SelectList(translatorsResult.Value, nameof(Translator.Id), nameof(Translator.Name));

            var clientsResult = await clientService.GetAll();
            ClientsList = new SelectList(clientsResult.Value, nameof(Client.Id), nameof(Client.Name));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var options = new RestClientOptions(apiConfig.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            var client = new RestClient(options);
            var request = new RestRequest("/Document/Update", Method.Put);
            var payload = new
            {
                Document.Id,
                Document.Title,
                Document.SignsSize,
                Document.CreatedAt,
                Document.Deadline,
                Document.ClientId,
                Document.TranslatorId
            };
            request.AddBody(payload);

            var response = await client.ExecuteAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Documents/Index");
            }
            else if (response.Content is not null)
            {
                var errors = JObject.Parse(response.Content)["Errors"];
                if (errors is not null)
                    foreach (var error in errors)
                    {
                        string propertyName = $"{nameof(Document)}.{error["propertyName"]}";
                        string errorMessage = error["errorMessage"]!.ToString();
                        ModelState.TryAddModelError(propertyName, errorMessage);
                    }


                var translatorsResult = await translatorService.GetAll();
                TranslatorsList = new SelectList(translatorsResult.Value, nameof(Translator.Id), nameof(Translator.Name));

                var clientsResult = await clientService.GetAll();
                ClientsList = new SelectList(clientsResult.Value, nameof(Client.Id), nameof(Client.Name));
            }
            return Page();
        }
    }
}