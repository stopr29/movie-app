using Microsoft.EntityFrameworkCore;
using Moq;
using MovieApp.Domain.Commands.Users;
using MovieApp.Domain.Models;
using MovieApp.Infrastructure.Auth;
using MovieApp.Infrastructure.Persistence;
using MovieApp.Services.Users;
using System.Security.Cryptography;
using System.Text;

namespace MovieApp.UnitTests.Auth;

public class AuthServiceTests
{
    private readonly Mock<IJwtTokenGenerator> _tokenGeneratorMock = new();
    private readonly MovieAppDbContext _dbContext;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _dbContext = TestDbContextFactory.CreateInMemoryDbContext();
        _authService = new AuthService(_dbContext, _tokenGeneratorMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_WithNewEmail_ShouldCreateUserAndReturnToken()
    {
        var command = new RegisterUserCommand
        {
            Email = "test@example.com",
            Username = "testuser",
            Password = "Password123!"
        };

        _tokenGeneratorMock.Setup(t => t.GenerateToken(It.IsAny<User>()))
                           .Returns("fake-jwt-token");

        var result = await _authService.RegisterAsync(command, CancellationToken.None);

        Assert.Equal("testuser", result.Username);
        Assert.Equal("fake-jwt-token", result.Token);

        var userInDb = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == command.Email);
        Assert.NotNull(userInDb);
        Assert.Equal(command.Username, userInDb.Username);
    }

    [Fact]
    public async Task RegisterAsync_WithExistingEmail_ShouldThrowException()
    {
        var existingUser = new User
        {
            Email = "test@example.com",
            Username = "existing",
            PasswordHash = [1, 2, 3],
            PasswordSalt = [1, 2, 3]
        };

        _dbContext.Users.Add(existingUser);
        await _dbContext.SaveChangesAsync();

        var command = new RegisterUserCommand
        {
            Email = "test@example.com",
            Username = "newuser",
            Password = "AnotherPass123!"
        };

        var ex = await Assert.ThrowsAsync<Exception>(() => _authService.RegisterAsync(command, CancellationToken.None));
        Assert.Equal("Email already in use.", ex.Message);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
    {
        RegisterAsync("Password123!", out var hash, out var salt);

        var user = new User
        {
            Email = "login@example.com",
            Username = "logintest",
            PasswordHash = hash,
            PasswordSalt = salt
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        _tokenGeneratorMock.Setup(t => t.GenerateToken(It.IsAny<User>()))
                           .Returns("jwt-login");

        var command = new LoginUserCommand
        {
            Email = "login@example.com",
            Password = "Password123!"
        };

        var result = await _authService.LoginAsync(command, CancellationToken.None);

        Assert.Equal("jwt-login", result.Token);
        Assert.Equal("logintest", result.Username);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidEmail_ShouldThrow()
    {
        var command = new LoginUserCommand
        {
            Email = "notfound@example.com",
            Password = "WrongPassword"
        };

        var ex = await Assert.ThrowsAsync<Exception>(() => _authService.LoginAsync(command, CancellationToken.None));
        Assert.Equal("Invalid credentials", ex.Message);
    }

    [Fact]
    public async Task LoginAsync_WithWrongPassword_ShouldThrow()
    {
        RegisterAsync("CorrectPass", out var hash, out var salt);

        var user = new User
        {
            Email = "wrongpass@example.com",
            Username = "user",
            PasswordHash = hash,
            PasswordSalt = salt
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        var command = new LoginUserCommand
        {
            Email = "wrongpass@example.com",
            Password = "WrongPass!"
        };

        var ex = await Assert.ThrowsAsync<Exception>(() => _authService.LoginAsync(command, CancellationToken.None));
        Assert.Equal("Invalid credentials", ex.Message);
    }

    private static void RegisterAsync(string password, out byte[] hash, out byte[] salt)
    {
        using var hmac = new HMACSHA256();
        salt = hmac.Key;
        hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}