using ExpenseCareApi.Core.Interfaces;
using ExpenseCareApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ExpenseCareApi.Core.Models;

namespace ExpenseCareApi.Infrastructure.Repositories;

public class DonationRepository : IDonationRepository
{
    private readonly ExpenseCareDbContext _context;
    public DonationRepository(ExpenseCareDbContext context)
    {
        _context = context;
    }

    public async Task<DonationDetails> CreateAsync(DonationDetails entity)
    {
        _context.Donations.Add(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<DonationDetails?> GetByIdAsync(int id)
    {
        return await _context.Donations.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<DonationDetails>> GetAllAsync()
    {
        return await _context.Donations
                  .OrderByDescending(x => x.DonationDate)
                  .ToListAsync();
    }
    public async Task<List<DonationDetails>> GetByMonthAsync(int year, int month)
    {
        var donations = await _context.Donations
        .Where(x => x.DonationDate.HasValue &&
                    x.DonationDate.Value.Year == year &&
                    x.DonationDate.Value.Month == month)
        .OrderByDescending(x => x.DonationDate)
        .ToListAsync();

        return donations;
    }
    public async Task<List<DonationDetails>> GetByYearAsync(int year)
    {
        return await _context.Donations
         .Where(x => x.DonationDate.HasValue &&
                     x.DonationDate.Value.Year == year)
         .OrderByDescending(x => x.DonationDate)
         .ToListAsync();

    }
    public async Task<List<DonationDetails>> GetPendingAsync()
    {
        return await _context.Donations
            .Where(x => x.Status == "Pending")
            .OrderByDescending(x => x.EntryAt)
            .ToListAsync();
    }

    public async Task UpdateAsync(DonationDetails entity)
    {
        _context.Donations.Update(entity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(DonationDetails entity)
    {
        _context.Donations.Remove(entity);
        await _context.SaveChangesAsync();
    }
    public async Task ApproveAsync(int id, string approvedBy)
    {
        var entity = await _context.Donations.FindAsync(id);
        if (entity == null) return;

        entity.Status = "Approved";
        entity.ApprovedBy = approvedBy;
        entity.ApprovedAt = DateTime.UtcNow.ToString("yyyy-MM-dd"); // string in your model

        await _context.SaveChangesAsync();
    }
    public async Task<List<DonationDetails>> GetByUserIdAsync(int userId)
    {
        return await _context.Donations
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.EntryAt)
            .ToListAsync();
    }

}