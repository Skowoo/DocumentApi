﻿namespace DocumentApi.Application.Interfaces
{
    public interface IUserService
    {
        public (bool, IList<string>?) AuthorizeUser(string login, string password);

        public bool RegisterUser(string login, string password);

        public bool AddRole(string roleName);

        public bool RemoveRole(string roleName);

        public bool AssignUserToRole(string userName, string roleName);

        public bool RemoveUserFromRole(string userName, string roleName);
    }
}
