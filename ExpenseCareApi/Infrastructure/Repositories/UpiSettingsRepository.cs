using ExpenseCareApi.Core.DTOs;
using ExpenseCareApi.Core.Interfaces;
using ExpenseCareApi.Core.Models;
using ExpenseCareApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseCareApi.Infrastructure.Repositories;

public class UpiSettingsRepository : IUpiSettingsRepository
{
    private readonly ExpenseCareDbContext _context;
    public UpiSettingsRepository(ExpenseCareDbContext context)
    {
        _context = context;
    }

    public async Task<UpiSettings?> GetAsync()
    {
        return await _context.UpiSettings.FirstOrDefaultAsync();
    }

    public async Task SaveAsync(UpiSettingsDto dto)
    {
        var existing = await _context.UpiSettings.FirstOrDefaultAsync();
        if (existing == null)
        {
            _context.UpiSettings.Add(new UpiSettings
            {
                UpiId             = dto.UpiId,
                DisplayName       = dto.DisplayName,
                BankName          = dto.BankName,
                BankAccountNumber = dto.BankAccountNumber,
                BankIfscCode      = dto.BankIfscCode,
                AccountHolderName = dto.AccountHolderName,
                UpdatedAt         = DateTime.UtcNow
            });
        }
        else
        {
            existing.UpiId             = dto.UpiId;
            existing.DisplayName       = dto.DisplayName;
            existing.BankName          = dto.BankName;
            existing.BankAccountNumber = dto.BankAccountNumber;
            existing.BankIfscCode      = dto.BankIfscCode;
            existing.AccountHolderName = dto.AccountHolderName;
            existing.UpdatedAt         = DateTime.UtcNow;
        }
        await _context.SaveChangesAsync();
    }
}