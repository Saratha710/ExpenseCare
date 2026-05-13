using Microsoft.EntityFrameworkCore.Internal;

namespace ExpenseCareApi.Core.DTOs;

public class RequestOtpDto
{
    public string MobileNumber { get; set; }
}

public class VerifyOtpDto
{
    public string MobileNumber { get; set; }
    public string Otp { get; set; }
}

public class UserLoginDto
{
    public string Identifier { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; } = string.Empty;
}

public class CreateOrderDto
{
    public decimal Amount { get; set; }
    public int UserId { get; set; }
    public string DonorName { get; set; } = string.Empty;
}

public class RegisterUserDto
{
    public string FullName { get; set; } = string.Empty;
    public string? UserName { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public string? Address { get; set; } = string.Empty;
}

public class UserDonorDto
{
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Mobile { get; set; }
    public string? Address { get; set; }
}

// RefreshTokenDto.cs
public class RefreshTokenDto
{
    public string RefreshToken { get; set; } = string.Empty;
}

// LogoutDto.cs
public class LogoutDto
{
    public int UserId { get; set; }
}