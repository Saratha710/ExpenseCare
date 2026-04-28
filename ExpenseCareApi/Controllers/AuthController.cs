using System;
using ExpenseCareApi.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseCareApi.Core.DTOs;
using ExpenseCareApi.Core.Interfaces;


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
            return NotFound(message);

        return Ok(new { message });
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


}