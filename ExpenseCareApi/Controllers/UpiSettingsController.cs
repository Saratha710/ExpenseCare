using ExpenseCareApi.Core.DTOs;
using ExpenseCareApi.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseCareApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UpiSettingsController : ControllerBase
{
    private readonly IUpiSettingsService _service;

    public UpiSettingsController(IUpiSettingsService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var dto = await _service.GetAsync();
        if (dto == null) return Ok(null);
        return Ok(dto);
    }
    
    [Authorize(Roles ="Admin")]
    [HttpPost]
    public async Task<IActionResult> Save([FromBody] UpiSettingsDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UpiId))
            return BadRequest(new { message = "UPI ID is required" });

        await _service.SaveAsync(dto);
        return Ok(new { message = "Settings saved successfully" });
    }
}