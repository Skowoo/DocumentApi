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

namespace ClientApplication.Pages.Documents
{
    public class EditModel(CurrentUser user, IOptions<DocumentApiConfig> apiConfig) : PageModel
    {
        [BindProperty]
        public Document Document { get; set; } = default!;

        public SelectList ClientsList { get; set; } = default!;

        public SelectList TranslatorsList { get; set; } = default!;

        public void OnGet(string id)
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

            List<Translator> allTranslators = [];
            options = new RestClientOptions(apiConfig.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            client = new RestClient(options);
            request = new RestRequest("/Translator/GetAll", Method.Get);
            response = client.ExecuteAsync(request).Result;
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                allTranslators = JsonConvert.DeserializeObject<List<Translator>>(response.Content!)!;
            }

            TranslatorsList = new SelectList(allTranslators, nameof(Translator.Id), nameof(Translator.Name));

            List<Client> allClients = [];
            options = new RestClientOptions(apiConfig.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            client = new RestClient(options);
            request = new RestRequest("/Client/GetAll", Method.Get);
            response = client.ExecuteAsync(request).Result;
            if (response.Content is not null && response.IsSuccessStatusCode)
            {
                allClients = JsonConvert.DeserializeObject<List<Client>>(response.Content)!;
            }

            ClientsList = new SelectList(allClients, nameof(Client.Id), nameof(Client.Name));
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

                List<Translator> allTranslators = [];
                options = new RestClientOptions(apiConfig.Value.FullApiUri)
                {
                    Authenticator = new JwtAuthenticator(user.Token!)
                };
                client = new RestClient(options);
                request = new RestRequest("/Translator/GetAll", Method.Get);
                response = client.ExecuteAsync(request).Result;
                if (response.IsSuccessStatusCode && response.Content is not null)
                {
                    allTranslators = JsonConvert.DeserializeObject<List<Translator>>(response.Content!)!;
                }

                TranslatorsList = new SelectList(allTranslators, nameof(Translator.Id), nameof(Translator.Name));

                List<Client> allClients = [];
                options = new RestClientOptions(apiConfig.Value.FullApiUri)
                {
                    Authenticator = new JwtAuthenticator(user.Token!)
                };
                client = new RestClient(options);
                request = new RestRequest("/Client/GetAll", Method.Get);
                response = client.ExecuteAsync(request).Result;
                if (response.Content is not null && response.IsSuccessStatusCode)
                {
                    allClients = JsonConvert.DeserializeObject<List<Client>>(response.Content)!;
                }

                ClientsList = new SelectList(allClients, nameof(Client.Id), nameof(Client.Name));
            }
            return Page();
        }
    }
}