using ExpenseCareApi.Core.Models;
using ExpenseCareApi.Core.DTOs;

namespace ExpenseCareApi.Core.Interfaces;

public interface IDonationRepository
{
    Task<DonationDetails> CreateAsync(DonationDetails entity);

    Task<DonationDetails?> GetByIdAsync(int id);
    Task<List<DonationDetails>> GetAllAsync();
    Task<List<DonationDetails>> GetByMonthAsync(int year, int month);
    Task<List<DonationDetails>> GetByYearAsync(int year);
    Task<List<DonationDetails>> GetPendingAsync();

    Task UpdateAsync(DonationDetails entity);
    Task DeleteAsync(DonationDetails entity);
    Task ApproveAsync(int id, string approveBy);
    Task<List<DonationDetails>> GetByUserIdAsync(string mobile);

    Task RejectAsync(int id, string rejectedBy);
    Task ApproveAllAsync(List<int> ids, string approvedBy);
    Task<UserDonorDto?> GetDonorByMobileAsync(string mobile);
}
