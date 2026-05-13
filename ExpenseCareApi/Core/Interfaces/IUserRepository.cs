using ExpenseCareApi.Core.Models;

namespace ExpenseCareApi.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByMobileAsync(string mobileNumber);

    Task SaveChangesAsync();

    Task<User?> GetByUserDetailsAsync(string identifier);

    Task<bool> ExistsAsync(string? userName, string? email, string mobileNumber);
    Task AddUserAsync(User user);

    Task SaveRefreshTokenAsync(int userId, string refreshToken, DateTime expiry);
    Task<User?> GetByRefreshTokenAsync(string refreshToken);

}