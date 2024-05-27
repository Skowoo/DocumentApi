namespace ClientApplication.Config
{
    public class JwtTokenStorage
    {
        string? _token = null;

        public bool IsValid => _token is not null;

        public void SetToken(string input) => _token = $"Bearer {input.Trim()[1..^1]}";

        public string? GetToken() => IsValid ? _token : null;
    }
}
