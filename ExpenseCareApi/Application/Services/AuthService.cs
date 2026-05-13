
using ExpenseCareApi.Core.DTOs;
using ExpenseCareApi.Core.Interfaces;
using BCrypt.Net;
using ExpenseCareApi.Core.Models;
using AutoMapper;

namespace ExpenseCareApi.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public AuthService(IUserRepository userRepo, ITokenService tokenService, IConfiguration config, IMapper mapper)
    {
        _userRepo = userRepo;
        _tokenService = tokenService;
        _config = config;
        _mapper = mapper;
        _mapper = mapper;
    }

    public async Task<(bool success, string message)> RequestOtpAsync(string mobileNumber)
    {
        var user = await _userRepo.GetByMobileAsync(mobileNumber);

        if (user == null)
            return (false, "User not found");

        if (user.Role != "Trustee" && user.Role != "Admin")
        {
            return (false, "Only registered Admin or Trustee can request OTP. Please check your mobile number.");
        }

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

        return (true, "Login successful", await BuildAuthResponseAsync(user));
    }

    public async Task<(bool success, string message, AuthResponseDto? data)> UserLoginAsync(UserLoginDto dto)
    {
        var user = await _userRepo.GetByUserDetailsAsync(dto.Identifier);

        if (user == null)
            return (false, "User not found. Please check your details.", null);

        if (user.Role != "User")
            return (false, "Invalid username or password", null);

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return (false, "Invalid password", null);

        return (true, "Login successful", await BuildAuthResponseAsync(user));
    }
    public async Task<(bool success, string message, AuthResponseDto? dto)> RegisterAsync(RegisterUserDto dto)
    {
        var exists = await _userRepo.ExistsAsync(dto.UserName, dto.Email, dto.MobileNumber);
        if (exists)
            return (false, "Username, mobile, or email already in use.", null);

        var user = _mapper.Map<User>(dto);

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        user.Role = "User";

        await _userRepo.AddUserAsync(user);

        return (true, "Account created successfully.", await BuildAuthResponseAsync(user));
    }
    public async Task<(bool success, string message, AuthResponseDto? data)> RefreshTokenAsync(string refreshToken)
    {
        var user = await _userRepo.GetByRefreshTokenAsync(refreshToken);

        if (user == null)
            return (false, "Invalid refresh token", null);

        if (user.RefreshTokenExpiry == null || DateTime.UtcNow > user.RefreshTokenExpiry)
            return (false, "Refresh token expired. Please login again.", null);

        // Issue new access token — refresh token stays the same until it expires
        var accessToken = _tokenService.GenerateAccessToken(user);

        return (true, "Token refreshed", await BuildAuthResponseAsync(user));
    }
    public async Task<bool> LogoutAsync(int userId)
    {
        await _userRepo.SaveRefreshTokenAsync(userId, string.Empty, DateTime.UtcNow);
        return true;
    }
    private async Task<AuthResponseDto> BuildAuthResponseAsync(User user)
    {
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var expiry = DateTime.UtcNow.AddDays(
                             int.Parse(_config["Jwt:RefreshTokenExpiryDays"]!));

        await _userRepo.SaveRefreshTokenAsync(user.Id, refreshToken, expiry);

        return new AuthResponseDto
        {
            UserId = user.Id,
            Name = user.Name,
            Mobile = user.MobileNumber,
            Role = user.Role,
            Email = user.Email,
            Address = user.Address,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
    


}