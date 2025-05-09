using MovieApp.Domain.Commands.Users;
using MovieApp.Domain.Results.Users;

namespace MovieApp.Services.Users;

public interface IAuthService
{
    Task<AuthResultDto> RegisterAsync(RegisterUserCommand command, CancellationToken token);
    Task<AuthResultDto> LoginAsync(LoginUserCommand command, CancellationToken token);
}