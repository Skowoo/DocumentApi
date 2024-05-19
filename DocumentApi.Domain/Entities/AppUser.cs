namespace DocumentApi.Domain.Entities
{
    public class AppUser(string login, string password)
    {
        public string Login { get; set; } = login;

        public string Password { get; set; } = password;
    }
}
