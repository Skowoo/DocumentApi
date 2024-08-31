using DocumentApi.Application.Clients.Commands.CreateClient;
using DocumentApi.Application.Clients.Commands.DeleteClient;
using DocumentApi.Application.Clients.Commands.UpdateClient;
using DocumentApi.Application.Clients.Queries.GetAllClients;
using DocumentApi.Application.Clients.Queries.GetClient;
using DocumentApi.Domain.Constants;
using DocumentApi.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApi.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = $"{Roles.User},{Roles.Administrator}")]
    public class ClientController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public Task<List<Client>> GetAll() => sender.Send(new GetAllClientsQuery());

        [HttpGet("{id}")]
        public Task<Client?> GetById(int id) => sender.Send(new GetClientQuery(id));

        [HttpPost]
        public Task<int> Add(CreateClientCommand command) => sender.Send(command);

        [HttpPut]
        public Task Update(UpdateClientCommand command) => sender.Send(command);

        [HttpDelete("{id}")]
        public Task Delete(int id) => sender.Send(new DeleteClientCommand(id));
    }
}
