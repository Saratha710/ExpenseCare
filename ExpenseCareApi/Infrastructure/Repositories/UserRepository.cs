
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

    public async Task<User?> GetByUserDetailsAsync(string identifier)
    {
        var lower = identifier.ToLower();
        return await _context.Users
            .FirstOrDefaultAsync(u => (u.UserName != null && u.UserName.ToLower() == lower) ||
                                            (u.Email != null && u.Email.ToLower() == lower) ||
                                            u.MobileNumber == identifier);
    }

    public async Task<bool> ExistsAsync(string? userName, string? email, string mobileNumber)
    {
        return await _context.Users
            .AnyAsync(u => (userName != null && !string.IsNullOrWhiteSpace(userName) && u.UserName == userName) ||
                           (email != null && !string.IsNullOrWhiteSpace(email) && u.Email == email) ||
                           u.MobileNumber == mobileNumber);
    }
    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task SaveRefreshTokenAsync(int userId, string refreshToken, DateTime expiry)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return;

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = expiry;
        user.LastAccessTime = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    }
}