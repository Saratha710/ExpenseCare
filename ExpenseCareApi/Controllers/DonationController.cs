using System;
using Microsoft.AspNetCore.Mvc;
using ExpenseCareApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ExpenseCareApi.Core.DTOs;
using AutoMapper;
using ExpenseCareApi.Core.Models;
using ExpenseCareApi.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseCareApi.Controllers;

[Authorize(Roles ="Admin,Trustee")]
[ApiController]
[Route("api/[controller]")]

public class DonationController : ControllerBase
{
    private readonly IDonationService _service;

    public DonationController(IDonationService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Admin,Trustee,User")]
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

        var success = await _service.UpdateAsync(id, dto);
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
        if (month < 1 || month > 12) return BadRequest("Invalid month");

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

   [Authorize(Roles ="Admin")]
    // PUT /api/donation/approve/{id}
    [HttpPut("approve/{id}")]
    public async Task<IActionResult> Approve(int id, [FromBody] ApproveDto dto)
    {
        var entity = await _service.ApproveAsync(id, dto.ApprovedBy);

        return Ok();
    }

   [Authorize(Roles = "Admin,Trustee,User")]
    [HttpGet("my-donations/{mobile}")]
    public async Task<IActionResult> GetMyDonations(string mobile)
    {
        //if (mobile <= 0) return BadRequest("Invalid userId");
        var result = await _service.GetByUserIdAsync(mobile);
        return Ok(result);
    }
    
 [Authorize(Roles = "Admin")]
[HttpPut("reject/{id}")]
    public async Task<IActionResult> Reject(int id, [FromBody] RejectDto dto)
    {
        var success = await _service.RejectAsync(id, dto.RejectedBy);
        if (!success) return NotFound("Donation not found");
        return Ok(new { message = "Donation rejected" });
    }

 [Authorize(Roles ="Admin")]
    [HttpPut("approve-all")]
    public async Task<IActionResult> ApproveAll([FromBody] ApproveAllDto dto)
    {
        await _service.ApproveAllAsync(dto.Ids, dto.ApprovedBy);
        return Ok(new { message = $"{dto.Ids.Count} donations approved" });
    }

    [HttpGet("donor-by-mobile/{mobile}")]
    public async Task<IActionResult> GetDonorByMobile(string mobile)
    {
        var donor = await _service.GetDonorByMobileAsync(mobile);
        return Ok(donor); // returns null if not found — frontend handles both
    }


}
