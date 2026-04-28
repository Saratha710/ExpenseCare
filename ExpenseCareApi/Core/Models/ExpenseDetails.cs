using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ExpenseCareApi.Core.Models;

[Table("ExpenseDetails")]
public class ExpenseDetails
{
    [Key] public int Id { get; set; }
    [Required] public int UserId { get; set; }
    [Required, MaxLength(100)] public string ExpenseType { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }
    public DateTime? ExpenseDate { get; set; }
    [Required, MaxLength(300)] public string Description { get; set; }
    [Required] public string Status { get; set; } = "Pending";
    [MaxLength(100)] public string? ApprovedBy { get; set; }
    public string? ApprovedAt { get; set; }
    public string? AttachImage{ get; set; }
    [MaxLength(2000)] public string? Notes { get; set; }
    [Required, MaxLength(100)] public string EntryBy { get; set; }
    [Required] public DateTime EntryAt { get; set; } = DateTime.Today; 

    //public virtual Transactions Transactions { get; set; }
}