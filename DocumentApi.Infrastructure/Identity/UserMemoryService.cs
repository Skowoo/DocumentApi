using DocumentApi.Application.Interfaces;

namespace DocumentApi.Infrastructure.Identity
{
    public class UserMemoryService : IUserService
    {
        readonly Dictionary<string, string> Users;

        public UserMemoryService()
        {
            Users = new Dictionary<string, string>
            {
                { "Admin", "AdminPass" },
                { "User", "UserPass" },
                { "User2", "User2Pass" }
            };
        }

        public bool AuthorizeUser(string login, string givenPassword)
        {
            if (Users.TryGetValue(login, out var savedPassword))
                if (savedPassword == givenPassword)
                    return true;

            return false;
        }
    }
}
