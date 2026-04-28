using System.ComponentModel.DataAnnotations;

namespace ExpenseCareApi.Core.DTOs;

public class CreateExpenseDto
{
    public string ExpenseType { get; set; }
    public decimal Amount { get; set; }
    public DateTime? ExpenseDate { get; set; }
    public string Description { get; set; }
    public string? AttachImage { get; set; }
    public string? Notes { get; set; }

}
public class UpdateExpenseDto
{
    public string ExpenseType { get; set; }
    public decimal Amount { get; set; }
    public DateTime? ExpenseDate { get; set; }
    public string Description { get; set; }
    public string? AttachImage { get; set; }
    public string? Notes { get; set; }

}
public class GetExpenseDetailsDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string ExpenseType { get; set; }
    public decimal Amount { get; set; }
    public DateTime? ExpenseDate { get; set; }
    public string Description { get; set; }
    public string Status { get; set; } = "Pending";
    public string? ApprovedBy { get; set; }
    public string? ApprovedAt { get; set; }
    public string? AttachImage { get; set; }
    public string? Notes { get; set; }
    public string EntryBy { get; set; }
    public DateTime EntryAt { get; set; } = DateTime.Today;
}
public class ExpenseResponseDto
{
    public string ExpenseType { get; set; }
    public decimal Amount { get; set; }
    public string StatusLabel { get; set; } 
    public bool CanEdit { get; set; }  
}