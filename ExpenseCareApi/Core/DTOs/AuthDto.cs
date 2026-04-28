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
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    // JWT READY
    // public string Token        { get; set; } = string.Empty;
    // public string RefreshToken { get; set; } = string.Empty;
}

public class CreateOrderDto
{
    public decimal Amount   { get; set; }
    public int     UserId   { get; set; }
    public string  DonorName { get; set; } = string.Empty;
}