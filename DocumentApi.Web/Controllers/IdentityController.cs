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
        public IActionResult Register(AppUser user) => identityService.RegisterUser(user.Login!, user.Password!) ? Ok() : BadRequest("Creation failed!");

        [HttpPost]
        public IActionResult AddRole(string roleName) => identityService.AddRole(roleName) ? Ok(roleName) : BadRequest();

        [HttpPost]
        public IActionResult RemoveRole(string roleName) => identityService.RemoveRole(roleName) ? Ok(roleName) : BadRequest();

        [HttpPost]
        public IActionResult AddUserToRole(string userName, string roleName) => identityService.AssignUserToRole(userName, roleName) ? Ok(roleName) : BadRequest();

        [HttpPost]
        public IActionResult RemoveUserFromRole(string userName, string roleName) => identityService.RemoveUserFromRole(userName, roleName) ? Ok(roleName) : BadRequest();
    }
}
