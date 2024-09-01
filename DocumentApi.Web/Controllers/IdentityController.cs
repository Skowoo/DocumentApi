using DocumentApi.Application.Common.Interfaces;
using DocumentApi.Domain.Constants;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApi.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = Roles.Administrator)]
    public class IdentityController(IUserService identityService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), 400)]
        public async Task<IActionResult> Login(AppUser user)
        {
            var (result, token) = await identityService.AuthorizeUser(user.Login, user.Password);
            return result.Succeeded ? Ok(token) : BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), 400)]
        public async Task<IActionResult> Register(AppUser user)
        {
            var (Result, UserId) = await identityService.RegisterUser(user.Login, user.Password);
            return Result.Succeeded ? Ok(UserId) : BadRequest(Result.Errors);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), 400)]
        public async Task<IActionResult> AddRole(string roleName)
        {
            var result = await identityService.AddRole(roleName);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), 400)]
        public async Task<IActionResult> RemoveRole(string roleName)
        {
            var result = await identityService.RemoveRole(roleName);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), 400)]
        public async Task<IActionResult> AddUserToRole(AppUser user)
        {
            var result = await identityService.AssignUserToRole(user.Login, user.Password);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), 400)]
        public async Task<IActionResult> RemoveUserFromRole(AppUser user)
        {
            var result = await identityService.RemoveUserFromRole(user.Login, user.Password);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<List<IdentityUser>>), 200)]
        public async Task<IActionResult> GetAllUsers() => Ok(await identityService.GetAllUsers());

        [HttpGet]
        [ProducesResponseType(typeof(IdentityUser), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> GetUserById(string id)
        {
            var result = await identityService.GetUserById(id);
            return result is not null ? Ok(result) : NotFound("User not found!");
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<List<IdentityRole>>), 200)]
        public async Task<IActionResult> GetAllRoles() => Ok(await identityService.GetAllRoles());
    }
}
