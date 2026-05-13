using System;
using ExpenseCareApi.Core.DTOs;

namespace ExpenseCareApi.Core.Interfaces;

public interface IDonationService
{
    Task<GetDonationDetailsDto> CreateAsync(CreateDonationDetailsDto dto);

    Task<GetDonationDetailsDto?> GetByIdAsync(int id);
    Task<List<GetDonationDetailsDto>> GetAllAsync();
    Task<List<GetDonationDetailsDto>> GetByMonthAsync(int year, int month);
    Task<List<GetDonationDetailsDto>> GetByYearAsync(int year);
    Task<List<GetDonationDetailsDto>> GetPendingAsync();

    Task<bool> UpdateAsync(int id, UpdateDonationDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ApproveAsync(int id, string approvedBy);
    Task<List<GetDonationDetailsDto>> GetByUserIdAsync(string mobile);
    Task<bool> RejectAsync(int id, string rejectedBy);
    Task<bool> ApproveAllAsync(List<int> ids, string approvedBy);
    Task<UserDonorDto?> GetDonorByMobileAsync(string mobile);
}