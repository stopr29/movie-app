﻿namespace MovieApp.Domain.Commands.Users;

public class LoginUserCommand
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
