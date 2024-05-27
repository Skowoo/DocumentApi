using System.IdentityModel.Tokens.Jwt;

namespace ClientApplication.Config
{
    public class CurrentUser
    {
        public string? Id { get; private set; }
        
        public string? Login { get; private set; }

        public IEnumerable<string>? Roles { get; private set;}

        public string? Token { get; private set; }

        public bool IsValid => Id is not null && Login is not null && Roles is not null && Token is not null;

        public void LogInUser(string input) 
        {
            var token = input.Trim()[1..^1];
            Token = $"Bearer {input}";
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            Id = jwtSecurityToken.Claims.First(c => c.Type == "Id").Value;
            Login = jwtSecurityToken.Claims.First(c => c.Type == "sub").Value;
            Roles = jwtSecurityToken.Claims.Where(c => c.Type == "role").Select(x => x.Value).ToList();
        }

        public void LogOutUser() 
        {
            Id = null;            
            Login = null;
            Roles = null;
            Token = null;
        }

        public bool IsInRole(string role) => IsValid && Roles!.Contains(role);
    }
}
