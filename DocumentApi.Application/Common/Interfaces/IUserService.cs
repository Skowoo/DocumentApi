using Microsoft.AspNetCore.Identity;

namespace DocumentApi.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<string?> AuthorizeUser(string login, string password);

        Task<(IdentityResult Result, string? UserId)> RegisterUser(string login, string password);

        Task<IdentityResult> AddRole(string roleName);

        Task<IdentityResult> RemoveRole(string roleName);

        Task<IdentityResult> AssignUserToRole(string userName, string roleName);

        Task<IdentityResult> RemoveUserFromRole(string userName, string roleName);
               
        Task<List<IdentityUser>> GetAllUsers();

        Task<IdentityUser?> GetUserById(string id);

        Task<List<IdentityRole>> GetAllRoles();       
    }
}
