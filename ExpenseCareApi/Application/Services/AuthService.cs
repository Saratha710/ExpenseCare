
using ExpenseCareApi.Core.DTOs;
using ExpenseCareApi.Core.Interfaces;
using BCrypt.Net;

namespace ExpenseCareApi.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;

    public AuthService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<(bool success, string message)> RequestOtpAsync(string mobileNumber)
    {
        var user = await _userRepo.GetByMobileAsync(mobileNumber);

        if (user == null)
            return (false, "User not found");

        var otp = "1234";
        // replace with: new Random().Next(100000,999999).ToString()
        user.Otp = otp;
        user.OtpExpiry = DateTime.UtcNow.AddMinutes(5);

        await _userRepo.SaveChangesAsync();

        // SMS READY: inject SmsService here when needed
        // await _smsService.SendOtp(mobileNumber, otp);

        return (true, "OTP sent successfully");
    }

    public async Task<(bool success, string message, AuthResponseDto? data)> VerifyOtpAsync(string mobileNumber, string otp)
    {
        var user = await _userRepo.GetByMobileAsync(mobileNumber);

        if (user == null)
            return (false, "User not found", null);

        if (user.Otp != otp)
            return (false, "Invalid OTP", null);

        if (user.OtpExpiry == null || DateTime.UtcNow > user.OtpExpiry)
            return (false, "OTP expired", null);

        // clear OTP after successful verify 
        user.Otp = null;
        user.OtpExpiry = null;
        await _userRepo.SaveChangesAsync();

        var response = new AuthResponseDto
        {
            UserId = user.Id,
            Name = user.Name,
            Mobile = user.MobileNumber,
            Role = user.Role
        };

        return (true, "Login successful", response);
    }
    
    public async Task<(bool success, string message, AuthResponseDto? data)> UserLoginAsync(UserLoginDto dto)
{
    var user = await _userRepo.GetByUserNameAsync(dto.UserName);

    if (user == null)
        return (false, "Invalid username or password", null);

    if (user.Role != "User")
        return (false, "Invalid username or password", null);

    if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return (false, "Invalid username or password", null);

    var response = new AuthResponseDto
    {
        UserId = user.Id,
        Name   = user.Name,
        Mobile = user.MobileNumber,
        Role   = user.Role
    };

    return (true, "Login successful", response);
}
}