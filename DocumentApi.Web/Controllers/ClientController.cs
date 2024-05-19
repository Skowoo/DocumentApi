using DocumentApi.Application.Clients.Commands.CreateClient;
using DocumentApi.Application.Clients.Commands.DeleteClient;
using DocumentApi.Application.Clients.Commands.UpdateClient;
using DocumentApi.Application.Clients.Queries.GetAllClients;
using DocumentApi.Application.Clients.Queries.GetClient;
using DocumentApi.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApi.Web.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientController() : ControllerBase
    {
        [HttpGet]
        public Task<List<Client>> GetAll(ISender sender) => sender.Send(new GetAllClientsQuery());

        [HttpGet("{id}")]
        public Task<Client?> GetById(ISender sender, int id) => sender.Send(new GetClientQuery(id));

        [HttpPost]
        public Task Add(ISender sender, CreateClientCommand command) => sender.Send(command);

        [HttpPut]
        public Task Update(ISender sender, UpdateClientCommand command) => sender.Send(command);

        [HttpDelete("{id}")]
        public Task Delete(ISender sender, int id) => sender.Send(new DeleteClientCommand(id));
    }
}
