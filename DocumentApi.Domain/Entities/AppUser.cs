namespace DocumentApi.Domain.Entities
{
    public class AppUser(string login, string password)
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Login { get; set; } = login;

        public string Password { get; set; } = password;

        public string? Name { get; set; }

        public IEnumerable<string>? Roles { get; set; }
    }
}
