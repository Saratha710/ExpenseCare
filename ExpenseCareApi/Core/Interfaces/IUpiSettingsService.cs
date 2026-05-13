using ExpenseCareApi.Core.DTOs;

namespace ExpenseCareApi.Core.Interfaces;

public interface IUpiSettingsService
{
    Task<UpiSettingsDto?> GetAsync();
    Task SaveAsync(UpiSettingsDto dto);
}