using MovieApp.Domain.Models;
using System.Security.Cryptography;
using System.Text;
using MovieApp.Domain.Results.Users;
using MovieApp.Domain.Commands.Users;
using MovieApp.Infrastructure.Auth;
using MovieApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MovieApp.Services.Users;

public class AuthService : IAuthService
{
    private readonly MovieAppDbContext _context;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public AuthService(MovieAppDbContext context, IJwtTokenGenerator tokenGenerator)
    {
        _context = context;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<AuthResultDto> RegisterAsync(RegisterUserCommand command, CancellationToken token)
    {
        if (await _context.Users.AnyAsync(u => u.Email == command.Email, token))
        {
            throw new Exception("Email already in use.");
        }

        CreatePasswordHash(command.Password, out var hash, out var salt);

        var user = new User
        {
            Email = command.Email,
            Username = command.Username,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(token);

        var jwt = _tokenGenerator.GenerateToken(user);

        return new AuthResultDto { Token = jwt, Username = user.Username };
    }

    public async Task<AuthResultDto> LoginAsync(LoginUserCommand command, CancellationToken token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == command.Email, token);
        if (user == null || !VerifyPassword(command.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new Exception("Invalid credentials");
        }

        var jwt = _tokenGenerator.GenerateToken(user);

        return new AuthResultDto { Token = jwt, Username = user.Username };
    }

    private static void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
    {
        using var hmac = new HMACSHA256();
        salt = hmac.Key;
        hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private static bool VerifyPassword(string password, byte[] hash, byte[] salt)
    {
        using var hmac = new HMACSHA256(salt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(hash);
    }
}