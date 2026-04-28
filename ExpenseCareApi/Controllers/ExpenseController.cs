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

public class ExpenseController : ControllerBase
{
    private readonly IExpenseService _service;
    public ExpenseController(IExpenseService service)
    {
        _service = service;
    }

    [HttpPost("add-expense")]
    public async Task<IActionResult> AddExpenseDetails([FromBody] CreateExpenseDto dto)
    {

        var expense = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetExpenseById), new { id = expense.Id }, expense);
    }

    [HttpGet("get-expense/{Id}")]
    public async Task<IActionResult> GetExpenseById(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid Id");

        var expense = await _service.GetByIdAsync(id);

        if (expense == null) return NotFound("Expense not found");

        return Ok(expense);
    }

    [HttpPut("update-expense/{id}")]
    public async Task<IActionResult> UpdateExpense(int id, [FromBody] UpdateExpenseDto dto)
    {
        if (id <= 0)
            return BadRequest();

        var success = await _service.UpdateAsync(id, dto);

        if (!success) return NotFound("Donation not found");

        return Ok();
    }

    [HttpGet("get-allExpenses")]
    public async Task<IActionResult> GetAllExpenseDetails()
    {

        var expenses = await _service.GetAllAsync();
        return Ok(expenses);

    }

    [HttpDelete("delete-Expense/{id}")]
    public async Task<IActionResult> DeleteExpense(int id)
    {

        if (id <= 0)
            return BadRequest();

        var success = await _service.DeleteAsync(id);
        if (!success)
            return NotFound("Expense not found");

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

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var expenses = await _service.GetPendingAsync();

        return Ok(expenses);
    }


    [HttpPut("approve/{id}")]
    public async Task<IActionResult> Approve(int id, [FromBody] ApproveDto dto)
    {
       var entity = await _service.ApproveAsync(id, dto.ApprovedBy);
        return Ok();
    }


 
 
}
