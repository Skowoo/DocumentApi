using DocumentApi.Application.Common.Interfaces;
using DocumentApi.Domain.Constants;
using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> Login(AppUser user)
        {
            var (result, token) = await identityService.AuthorizeUser(user.Login, user.Password);
            return result.Succeeded ? Ok(token) : BadRequest(result.Errors);
        }

        [AllowAnonymous]
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
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveRole(string roleName)
        {
            var result = await identityService.RemoveRole(roleName);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(AppUser user)
        {
            var result = await identityService.AssignUserToRole(user.Login, user.Password);
            return result.Succeeded? Ok(result) : BadRequest(result);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUserFromRole(AppUser user)
        {
            var result = await identityService.RemoveUserFromRole(user.Login, user.Password);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers() => Ok(await identityService.GetAllUsers());

        [HttpGet]
        public async Task<IActionResult> GetUserById(string id)
        {
            var result = await identityService.GetUserById(id);
            return result is not null ? Ok(result) : BadRequest("User not found!");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles() => Ok(await identityService.GetAllRoles());
    }
}
