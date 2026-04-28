using System;
using ExpenseCareApi.Core.DTOs;

namespace ExpenseCareApi.Core.Interfaces;

public interface IAuthService
{
    Task<(bool success, string message)> RequestOtpAsync(string mobileNumber);
    Task<(bool success, string message, AuthResponseDto? data)> VerifyOtpAsync(string mobileNumber, string otp);
    Task<(bool success, string message, AuthResponseDto? data)> UserLoginAsync(UserLoginDto dto);
}