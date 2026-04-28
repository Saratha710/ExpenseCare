using ExpenseCareApi.Core.Models;

namespace ExpenseCareApi.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByMobileAsync(string mobileNumber);

    Task SaveChangesAsync();

    Task<User?> GetByUserNameAsync(string userName);
}