using System;
using ExpenseCareApi.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseCareApi.Core.DTOs;
using ExpenseCareApi.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace ExpenseCareApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{

    private readonly IAuthService _service;

    private readonly SmsService _smsService;



    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("request-otp")]
    public async Task<IActionResult> RequestOtp([FromBody] RequestOtpDto dto)
    {
        var (success, message) = await _service.RequestOtpAsync(dto.MobileNumber);

        if (!success)
            return BadRequest(message);

        return Ok(message);
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
    {
        var (success, message, data) = await _service.VerifyOtpAsync(dto.MobileNumber, dto.Otp);

        if (!success)
            return BadRequest(new { message });

        return Ok(data);

    }
    [HttpPost("user-login")]
    public async Task<IActionResult> UserLogin([FromBody] UserLoginDto dto)
    {
        var (success, message, data) = await _service.UserLoginAsync(dto);

        if (!success)
            return Unauthorized(new { message });

        return Ok(data);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        if (dto.UserName == null || string.IsNullOrWhiteSpace(dto.UserName))
        {
            dto.UserName = dto.FullName;
        }
        var (success, message, data) = await _service.RegisterAsync(dto);
        if (!success) return BadRequest(new { message });
        return Ok(new { message, data });
    }

[HttpPost("refresh")]
public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
{
    var (success, message, data) = await _service.RefreshTokenAsync(dto.RefreshToken);
    if (!success) return Unauthorized(new { message });
    return Ok(data);
}

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutDto dto)
    {
        await _service.LogoutAsync(dto.UserId);
        return Ok(new { message = "Logged out successfully" });
    }

}