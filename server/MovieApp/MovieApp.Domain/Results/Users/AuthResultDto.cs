namespace MovieApp.Domain.Results.Users;

public class AuthResultDto
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}