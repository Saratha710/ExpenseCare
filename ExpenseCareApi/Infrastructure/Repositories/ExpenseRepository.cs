using ExpenseCareApi.Core.Interfaces;
using ExpenseCareApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ExpenseCareApi.Core.Models;

namespace ExpenseCareApi.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly ExpenseCareDbContext _context;
    public ExpenseRepository(ExpenseCareDbContext context)
    {
        _context = context;
    }

    public async Task<ExpenseDetails> CreateAsync(ExpenseDetails entity)
    {
        _context.Expenses.Add(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<ExpenseDetails?> GetByIdAsync(int id)
    {
        return await _context.Expenses.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<ExpenseDetails>> GetAllAsync()
    {
        return await _context.Expenses
                  .OrderByDescending(x => x.ExpenseDate)
                  .ToListAsync();
    }
    public async Task<List<ExpenseDetails>> GetByMonthAsync(int year, int month)
    {
        var expenses = await _context.Expenses
        .Where(x => x.ExpenseDate.HasValue &&
                    x.ExpenseDate.Value.Year == year &&
                    x.ExpenseDate.Value.Month == month)
        .OrderByDescending(x => x.ExpenseDate)
        .ToListAsync();

        return expenses;
    }
    public async Task<List<ExpenseDetails>> GetByYearAsync(int year)
    {
        return await _context.Expenses
         .Where(x => x.ExpenseDate.HasValue &&
                     x.ExpenseDate.Value.Year == year)
         .OrderByDescending(x => x.ExpenseDate)
         .ToListAsync();

    }
    public async Task<List<ExpenseDetails>> GetPendingAsync()
    {
        return await _context.Expenses
            .Where(x => x.Status == "Pending")
            .OrderByDescending(x => x.EntryAt)
            .ToListAsync();
    }

    public async Task UpdateAsync(ExpenseDetails entity)
    {
        _context.Expenses.Update(entity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(ExpenseDetails entity)
    {
        _context.Expenses.Remove(entity);
        await _context.SaveChangesAsync();
    }
    public async Task ApproveAsync(int id, string approvedBy)
    {
        var entity = await _context.Expenses.FindAsync(id);
        if (entity == null) return;

        entity.Status = "Approved";
        entity.ApprovedBy = approvedBy;
        entity.ApprovedAt = DateTime.UtcNow.ToString("yyyy-MM-dd");

        await _context.SaveChangesAsync();
    }
    public async Task RejectAsync(int id, string rejectedBy)
    {
        var entity = await _context.Expenses.FindAsync(id);
        if (entity == null) return;

        entity.Status = "Rejected";
        entity.ApprovedBy = rejectedBy;
        entity.ApprovedAt = DateTime.UtcNow.ToString("yyyy-MM-dd");

        await _context.SaveChangesAsync();
    }

    public async Task ApproveAllAsync(List<int> ids, string approvedBy)
    {
        var expenses = await _context.Expenses
            .Where(d => ids.Contains(d.Id))
            .ToListAsync();

        foreach (var d in expenses)
        {
            d.Status = "Approved";
            d.ApprovedBy = approvedBy;
            d.ApprovedAt = DateTime.UtcNow.ToString("yyyy-MM-dd");
        }

        await _context.SaveChangesAsync();
    }

}