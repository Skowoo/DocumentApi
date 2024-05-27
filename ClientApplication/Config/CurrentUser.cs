using System.IdentityModel.Tokens.Jwt;

namespace ClientApplication.Config
{
    public class CurrentUser
    {
        public string? Token = null;

        public string? Login { get; private set; }

        public IEnumerable<string>? Roles { get; private set;}

        public bool IsValid => Token is not null;

        public void LogUser(string input) 
        {
            var token = input.Trim()[1..^1];
            Token = $"Bearer {input}";            
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            Login = jwtSecurityToken.Claims.First(c => c.Type == "sub").Value;
            Roles = jwtSecurityToken.Claims.Where(c => c.Type == "role").Select(x => x.Value).ToList();
        }

        public void Logout() 
        {
            Token = null;
            Login = null;
            Roles = null;
        }
    }
}
