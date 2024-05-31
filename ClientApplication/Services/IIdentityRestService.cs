using DocumentApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ClientApplication.Services
{
    public interface IIdentityRestService
    {
        Task<(IdentityResult Result, string Id)> CreateUser(AppUser user);

        Task<IdentityResult> AssignUserToRole(string userName, string roleName);

        Task<IdentityResult> RemoveUserFromRole(string userName, string roleName);

        Task<List<IdentityUser>> GetAllUsers();

        Task<IdentityUser?> GetUserById(Guid Id);

        Task<List<IdentityRole>> GetAllRoles();
    }
}
