using ExpenseCareApi.Core.Models;

namespace ExpenseCareApi.Core.Interfaces;

public interface IExpenseRepository
{
    Task<ExpenseDetails> CreateAsync(ExpenseDetails entity);

    Task<ExpenseDetails?> GetByIdAsync(int id);
    Task<List<ExpenseDetails>> GetAllAsync();
    Task<List<ExpenseDetails>> GetByMonthAsync(int year, int month);
    Task<List<ExpenseDetails>> GetByYearAsync(int year);
    Task<List<ExpenseDetails>> GetPendingAsync();

    Task UpdateAsync(ExpenseDetails entity);
    Task DeleteAsync(ExpenseDetails entity);
    Task ApproveAsync(int id, string approveBy);

    Task RejectAsync(int id, string rejectedBy);
    Task ApproveAllAsync(List<int> ids, string approvedBy);
}