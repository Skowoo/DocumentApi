namespace DocumentApi.Application.Interfaces
{
    public interface IUserService
    {
        public bool AuthorizeUser(string login, string password);

        public void RegisterUser(string login, string password);
    }
}
