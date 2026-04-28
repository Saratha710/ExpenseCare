
namespace ExpenseCareApi.Core.Models;

using System;
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string MobileNumber { get; set; }
    public string Role { get; set; } = "User";

     //for user login
    public string? UserName     { get; set; }
    public string? PasswordHash { get; set; }

    //Token
    public bool IsLoggedIn { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
    public DateTime? LastAccessTime { get; set; }

    //Otp
    public string? Otp { get; set; }
    public DateTime? OtpExpiry { get; set; }
    
}