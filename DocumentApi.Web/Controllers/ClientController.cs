using DocumentApi.Application.Clients.Commands.CreateClient;
using DocumentApi.Application.Clients.Commands.DeleteClient;
using DocumentApi.Application.Clients.Commands.UpdateClient;
using DocumentApi.Application.Clients.Queries.GetAllClients;
using DocumentApi.Application.Clients.Queries.GetClient;
using DocumentApi.Domain.Constants;
using DocumentApi.Domain.Entities;
using FluentValidation.Results;
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
        [ProducesResponseType(typeof(List<Client>), 200)]
        public async Task<IActionResult> GetAll() => Ok(await sender.Send(new GetAllClientsQuery()));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Client), 200)]
        [ProducesResponseType(typeof(int), 404)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await sender.Send(new GetClientQuery(id));
            return result is not null ? Ok(result) : NotFound(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(ValidationFailure), 400)]
        public async Task<IActionResult> Add(CreateClientCommand command) => Ok(await sender.Send(command));

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ValidationFailure), 400)]
        public async Task<IActionResult> Update(UpdateClientCommand command)
        {
            await sender.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            await sender.Send(new DeleteClientCommand(id));
            return NoContent();
        }
    }
}
