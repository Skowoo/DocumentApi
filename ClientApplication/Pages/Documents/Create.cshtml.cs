using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using RestSharp.Authenticators;
using RestSharp;
using ClientApplication.Config;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Rendering;
using DocumentApi.Domain.Constants;
using Newtonsoft.Json;

namespace ClientApplication.Pages.Documents
{
    public class CreateModel(CurrentUser user, IOptions<DocumentApiConfig> apiConfig) : PageModel
    {
        [BindProperty]
        public Document Document { get; set; } = default!;

        public SelectList ClientsList { get; set; } = default!;

        public SelectList TranslatorsList { get; set; } = default!;

        public void OnGet() 
        {
            if (!user.IsInRole(Roles.User))
                RedirectToPage("/Index");

            List<Translator> allTranslators = [];
            var options = new RestClientOptions(apiConfig.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            var client = new RestClient(options);
            var request = new RestRequest("/Translator/GetAll", Method.Get);
            var response = client.ExecuteAsync(request).Result;
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
            Document.CreatedAt = DateTime.Now;

            var options = new RestClientOptions(apiConfig.Value.FullApiUri)
            {
                Authenticator = new JwtAuthenticator(user.Token!)
            };
            var client = new RestClient(options);
            var request = new RestRequest("/Document/Add", Method.Post);
            var payload = new
            {
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
