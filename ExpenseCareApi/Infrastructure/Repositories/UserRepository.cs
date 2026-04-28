
using ExpenseCareApi.Core.Interfaces;
using ExpenseCareApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ExpenseCareApi.Core.Models;

namespace ExpenseCareApi.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ExpenseCareDbContext _context;
    public UserRepository(ExpenseCareDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByMobileAsync(string mobileNumber)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.MobileNumber == mobileNumber);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName);
    }

}