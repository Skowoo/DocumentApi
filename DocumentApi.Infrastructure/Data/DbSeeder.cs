using DocumentApi.Domain.Constants;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentApi.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
            await initializer.Initialize();
        }
    }

    public class DbInitializer(DocumentDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        const string adminCredentials = "Admin";
        const string userCredentials = "User";

        public async Task Initialize()
        {
            await SeedUsers();
            await SeedRoles();
            await AssignRoles();
            await SeedClients();
            await SeedTranslators();
            await SeedDocuments(100);
        }

        private async Task SeedUsers()
        {
            if (await userManager.FindByNameAsync(adminCredentials) is null)
            {
                var user = new IdentityUser
                {
                    UserName = adminCredentials,
                };
                await userManager.CreateAsync(user, adminCredentials);
            }

            if (await userManager.FindByNameAsync(userCredentials) is null)
            {
                var user = new IdentityUser
                {
                    UserName = userCredentials,
                };
                await userManager.CreateAsync(user, userCredentials);
            }
        }

        private async Task SeedRoles()
        {
            if (!await roleManager.RoleExistsAsync(Roles.Administrator))
                await roleManager.CreateAsync(new IdentityRole(Roles.Administrator));

            if (!await roleManager.RoleExistsAsync(Roles.User))
                await roleManager.CreateAsync(new IdentityRole(Roles.User));
        }

        private async Task AssignRoles()
        {
            var admin = await userManager.FindByNameAsync(adminCredentials);
            var user = await userManager.FindByNameAsync("User");

            await userManager.AddToRoleAsync(admin!, Roles.Administrator);
            await userManager.AddToRoleAsync(user!, Roles.User);
        }

        private async Task SeedClients()
        {
            if (!context.Clients.Any())
            {
                List<Client> clients =
                [
                    new() {
                        Name = "Client 1",
                        Email = "first@tlen.pl",
                        TelephoneNumber = "123456789"
                    },
                    new() {
                        Name = "Client 2",
                        Email = "second@gmail.com",
                        TelephoneNumber = "234567891"
                    },
                    new() {
                        Name = "Client 3",
                        Email = "third@interia.pl",
                        TelephoneNumber = "345678912"
                    }
                ];
                await context.Clients.AddRangeAsync(clients);
                await context.SaveChangesAsync();
            }
        }

        private async Task SeedTranslators()
        {
            List<Translator> translators =
            [
                new(){
                    Name = "Tłumacz 1"
                },
                new(){
                    Name = "Tłumacz 2"
                },
                new(){
                    Name = "Tłumacz 3"
                },
            ];
            await context.Translators.AddRangeAsync(translators);
            await context.SaveChangesAsync();
        }

        private async Task SeedDocuments(int quantity)
        {
            Random random = new();
            int translatorsCount = context.Translators.Count(),
                clientsCount = context.Clients.Count();

            List<Document> documents = [];
            while (quantity-- > 0) 
            {
                documents.Add(new Document
                {
                    Title = $"Document {quantity}",
                    SignsSize = random.Next(10, 100_000),
                    CreatedAt = DateTime.Now.AddDays(random.Next(-10, 10)),
                    Deadline = DateTime.Now.AddDays(random.Next(15, 30)),
                    ClientId = random.Next(0, clientsCount),
                    TranslatorId = random.Next(0, translatorsCount * 2) > translatorsCount ? null : random.Next(0, translatorsCount),
                });
            }
            await context.Documents.AddRangeAsync(documents);
            await context.SaveChangesAsync();
        }
    }
}
