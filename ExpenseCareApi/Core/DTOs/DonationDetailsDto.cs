using System.ComponentModel.DataAnnotations;

namespace ExpenseCareApi.Core.DTOs;

public class CreateDonationDetailsDto
{
    public string DonorName { get; set; }
    public decimal Amount { get; set; }
    public string? DonorAddress { get; set; }
    public string? DonorMobile { get; set; }
    public string? DonationFor { get; set; }
    public DateTime? DonationDate { get; set; } = DateTime.Today;
    public string PaymentMode { get; set; }
    public string?   PaymentReference { get; set; }
    public string? Notes { get; set; }

}

public class UpdateDonationDto
{
    public string DonorName { get; set; }
    public decimal Amount { get; set; }
    public string? DonorAddress { get; set; }
    public string? DonorMobile { get; set; }
    public string? DonationFor { get; set; }
    public DateTime? DonationDate { get; set; } = DateTime.Today;
    public string PaymentMode { get; set; }
    public string? Notes { get; set; }

}

public class GetDonationDetailsDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string DonorName { get; set; } = string.Empty;
    public string? DonorMobile { get; set; }
    public string? DonorAddress { get; set; }
    public decimal Amount { get; set; }
    public DateTime? DonationDate { get; set; }
    public string PaymentMode { get; set; } = string.Empty;
    public string? DonationFor { get; set; }
    public string? Notes { get; set; }
    public string EntryBy { get; set; } = string.Empty;
    public DateTime EntryAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ApprovedBy { get; set; }
    public string? ApprovedAt { get; set; }
}

public class DonationResponseDto
{
    public string DonorName { get; set; }
    public decimal Amount { get; set; }
    public string StatusLabel { get; set; }
    public bool CanEdit { get; set; }
}
