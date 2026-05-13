namespace ExpenseCareApi.Core.DTOs;

public class UpiSettingsDto
{
    // UPI
    public string UpiId { get; set; } = string.Empty;
    public string? DisplayName { get; set; }

    // Bank
    public string? BankName { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? BankIfscCode { get; set; }
    public string? AccountHolderName { get; set; }
}