using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ExpenseCareApi.Core.Models;

[Table("DonationDetails")]
public class DonationDetails
{
    [Key] public int Id { get; set; }
    [Required] public int UserId { get; set; }
    [Required, MaxLength(200)] public string DonorName { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }
    [MaxLength(500)] public string? DonorAddress { get; set; }
    [MaxLength(20)] public string? DonorMobile { get; set; }
    [MaxLength(200)] public string? DonationFor { get; set; }
    public DateTime? DonationDate { get; set; }
    [Required, MaxLength(50)] public string PaymentMode { get; set; }
     public string?   PaymentReference { get; set; }
    public string? Notes { get; set; }
    [Required, MaxLength(100)] public string EntryBy { get; set; }
    [Required] public DateTime EntryAt { get; set; }= DateTime.Today; 
    [Required] public string Status { get; set; }
     public string? ApprovedBy { get; set; }
     public string? ApprovedAt { get; set; }
    

   // public virtual Transactions Transactions { get; set; }


}