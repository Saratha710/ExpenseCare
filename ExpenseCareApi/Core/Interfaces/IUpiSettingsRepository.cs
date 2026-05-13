namespace ExpenseCareApi.Core.Interfaces;

using ExpenseCareApi.Core.Models;
using ExpenseCareApi.Core.DTOs;

public interface IUpiSettingsRepository
{
    Task<UpiSettings?> GetAsync();
    Task SaveAsync(UpiSettingsDto dto);
}