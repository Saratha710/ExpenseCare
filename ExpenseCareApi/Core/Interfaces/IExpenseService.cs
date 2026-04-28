using System;
using ExpenseCareApi.Core.DTOs;

namespace ExpenseCareApi.Core.Interfaces;
public interface IExpenseService
{
    Task<GetExpenseDetailsDto> CreateAsync(CreateExpenseDto dto);

    Task<GetExpenseDetailsDto?> GetByIdAsync(int id);
    Task<List<GetExpenseDetailsDto>> GetAllAsync();
    Task<List<GetExpenseDetailsDto>> GetByMonthAsync(int year, int month);
    Task<List<GetExpenseDetailsDto>> GetByYearAsync(int year);
    Task<List<GetExpenseDetailsDto>> GetPendingAsync();

    Task<bool> UpdateAsync(int id, UpdateExpenseDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ApproveAsync(int id, string approvedBy);

}