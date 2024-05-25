using DocumentApi.Application.Common.Interfaces;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentApi.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class IdentityController(IUserService identityService) : ControllerBase
    {
        [HttpPost]
        public IActionResult Login(AppUser user)
        {
            var result = identityService.AuthorizeUser(user.Login, user.Password);
            if (result is not null)
                return Ok(result);

            return BadRequest("Login failed!");
        }

        [HttpPost]
        public async Task<IActionResult> Register(AppUser user)
        {
            var (Result, UserId) = await identityService.RegisterUser(user.Login, user.Password);
            return Result.Succeeded ? Ok(UserId) : BadRequest(Result.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            var result = await identityService.AddRole(roleName);
            return result.Succeeded ? Ok(roleName) : BadRequest(result.Errors);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveRole(string roleName)
        {
            var result = await identityService.RemoveRole(roleName);
            return result.Succeeded ? Ok(roleName) : BadRequest(result.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(string userName, string roleName)
        {
            var result = await identityService.AssignUserToRole(userName, roleName);
            return result.Succeeded? Ok() : BadRequest(result.Errors);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUserFromRole(string userName, string roleName)
        {
            var result = await identityService.RemoveUserFromRole(userName, roleName);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers() => Ok(await identityService.GetAllUsers());

        [HttpGet]
        public async Task<IActionResult> GetUserById(string id)
        {
            var result = await identityService.GetUserById(id);
            return result is null ? BadRequest("User not found!") : Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles() => Ok(await identityService.GetAllRoles());
    }
}
