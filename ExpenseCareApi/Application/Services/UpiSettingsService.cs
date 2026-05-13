using ExpenseCareApi.Core.DTOs;
using ExpenseCareApi.Core.Interfaces;

namespace ExpenseCareApi.Application.Services;

public class UpiSettingsService : IUpiSettingsService
{
    private readonly IUpiSettingsRepository _repo;

    public UpiSettingsService(IUpiSettingsRepository repo)
    {
        _repo = repo;
    }

    public async Task<UpiSettingsDto?> GetAsync()
    {
        var entity = await _repo.GetAsync();
        if (entity == null) return null;

        return new UpiSettingsDto
        {
            UpiId             = entity.UpiId,
            DisplayName       = entity.DisplayName,
            BankName          = entity.BankName,
            BankAccountNumber = entity.BankAccountNumber,
            BankIfscCode      = entity.BankIfscCode,
            AccountHolderName = entity.AccountHolderName
        };
    }

    public async Task SaveAsync(UpiSettingsDto dto)
    {
        await _repo.SaveAsync(dto);
    }
}