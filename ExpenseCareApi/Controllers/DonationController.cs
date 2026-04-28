using System;
using Microsoft.AspNetCore.Mvc;
using ExpenseCareApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ExpenseCareApi.Core.DTOs;
using AutoMapper;
using ExpenseCareApi.Core.Models;
using ExpenseCareApi.Core.Interfaces;

namespace ExpenseCareApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class DonationController : ControllerBase
{
    private readonly IDonationService _service;

    public DonationController(IDonationService service)
    {
        _service = service;
    }

    [HttpPost("add-donation")]
    public async Task<IActionResult> AddDonationDetails([FromBody] CreateDonationDetailsDto dto)
    {

        var donation = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetDonationById), new { id = donation.Id }, donation);
    }

    [HttpGet("get-Donation/{id}")]
    public async Task<IActionResult> GetDonationById(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid Id");

        var donation = await _service.GetByIdAsync(id);

        if (donation == null)
            return NotFound("Donation not found.");

        return Ok(donation);
    }

    [HttpPut("update-Donation/{id}")]
    public async Task<IActionResult> UpdateDonation(int id, [FromBody] UpdateDonationDto dto)
    {

        if (id <= 0)
            return BadRequest();

        var success = await _service.UpdateAsync(id,dto);
        if (!success) return NotFound("Donation not found");

        return Ok();
    }

    [HttpGet("get-allDonations")]
    public async Task<IActionResult> GetAllDonationDetails()
    {

        var donations = await _service.GetAllAsync();

        return Ok(donations);

    }

    [HttpDelete("delete-Donation/{id}")]
    public async Task<IActionResult> DeleteDonation(int id)
    {

        if (id <= 0)
            return BadRequest("Invalid Id");

        var success = await _service.DeleteAsync(id);

        if (!success)
            return NotFound("Donation not found");

        return Ok();
    }

    [HttpGet("by-month/{year}/{month}")]
    public async Task<IActionResult> GetByMonth(int year, int month)
    {
        if (year < 2000 || year > 2100) return BadRequest("Invalid year");
        if (month < 1   || month > 12)  return BadRequest("Invalid month");

        var result = await _service.GetByMonthAsync(year, month);
        return Ok(result);
    }

    [HttpGet("by-year/{year}")]
    public async Task<IActionResult> GetByYear(int year)
    {

        if (year < 2000 || year > 2100) return BadRequest("Invalid year");
        var result = await _service.GetByYearAsync(year);
        return Ok(result);
    }

    // GET /api/donation/pending
    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {

        var result = await _service.GetPendingAsync();
        return Ok(result);

    }


    // PUT /api/donation/approve/{id}
    [HttpPut("approve/{id}")]
    public async Task<IActionResult> Approve(int id, [FromBody] ApproveDto dto)
    {
        var entity = await _service.ApproveAsync(id, dto.ApprovedBy);

        return Ok();
    }


    [HttpGet("my-donations/{userId}")]
    public async Task<IActionResult> GetMyDonations(int userId)
    {
        if (userId <= 0) return BadRequest("Invalid userId");
        var result = await _service.GetByUserIdAsync(userId);
        return Ok(result);
    }

}
