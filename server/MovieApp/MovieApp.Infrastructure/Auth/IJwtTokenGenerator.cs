using MovieApp.Domain.Models;

namespace MovieApp.Infrastructure.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}