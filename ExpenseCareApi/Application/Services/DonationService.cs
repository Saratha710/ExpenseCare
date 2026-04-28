
using AutoMapper;
using ExpenseCareApi.Core.DTOs;
using ExpenseCareApi.Core.Interfaces;
using ExpenseCareApi.Core.Mapping;
using ExpenseCareApi.Core.Models;
using ExpenseCareApi.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpenseCareApi.Application.Services;

public class DonationService : IDonationService
{
    private readonly IDonationRepository _donationRepo;
    private readonly IMapper _mapper;

    public DonationService(IDonationRepository donationRepo, IMapper mapper)
    {
        _donationRepo = donationRepo;
        _mapper = mapper;
    }

    public async Task<GetDonationDetailsDto> CreateAsync(CreateDonationDetailsDto dto)
    {

        var entity = _mapper.Map<DonationDetails>(dto);

        entity.EntryAt = DateTime.UtcNow;
        //entity.EntryBy = _currentUser.Name;
        //entity.UserId = _currentUser.Id;
        entity.Status = "Pending";

        var result = await _donationRepo.CreateAsync(entity);
        return _mapper.Map<GetDonationDetailsDto>(result);
    }
    public async Task<GetDonationDetailsDto?> GetByIdAsync(int id)
    {
        var entity = await _donationRepo.GetByIdAsync(id);

        if (entity == null)
            return null;

        var result = _mapper.Map<GetDonationDetailsDto>(entity);

        return result;
    }

    public async Task<List<GetDonationDetailsDto>> GetAllAsync()
    {
        var entities = await _donationRepo.GetAllAsync();
        return _mapper.Map<List<GetDonationDetailsDto>>(entities);

    }
    public async Task<List<GetDonationDetailsDto>> GetByMonthAsync(int year, int month)
    {
        var entities = await _donationRepo.GetByMonthAsync(year, month);
        return _mapper.Map<List<GetDonationDetailsDto>>(entities);
    }
    public async Task<List<GetDonationDetailsDto>> GetByYearAsync(int year)
    {
        var entities = await _donationRepo.GetByYearAsync(year);
        return _mapper.Map<List<GetDonationDetailsDto>>(entities);
    }
    public async Task<List<GetDonationDetailsDto>> GetPendingAsync()
    {
        var entities = await _donationRepo.GetPendingAsync();
        return _mapper.Map<List<GetDonationDetailsDto>>(entities);
    }

    public async Task<bool> UpdateAsync(int id, UpdateDonationDto dto)
    {
        var entity = await _donationRepo.GetByIdAsync(id);

        if (entity == null)
            return false;

        _mapper.Map(dto, entity);

        await _donationRepo.UpdateAsync(entity);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _donationRepo.GetByIdAsync(id);

        if (entity == null)
            return false;

        await _donationRepo.DeleteAsync(entity);
        return true;
    }

    public async Task<bool> ApproveAsync(int id, string approvedBy)
    {
        var entity = await _donationRepo.GetByIdAsync(id);
        if (entity == null)
            return false;


        await _donationRepo.ApproveAsync(id, approvedBy);
        return true;
    }
    public async Task<List<GetDonationDetailsDto>> GetByUserIdAsync(int userId)
    {
        var entities = await _donationRepo.GetByUserIdAsync(userId);
        return _mapper.Map<List<GetDonationDetailsDto>>(entities);
    }

}