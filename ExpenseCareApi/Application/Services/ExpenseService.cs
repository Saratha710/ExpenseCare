
using AutoMapper;
using ExpenseCareApi.Core.DTOs;
using ExpenseCareApi.Core.Interfaces;
using ExpenseCareApi.Core.Mapping;
using ExpenseCareApi.Core.Models;
using ExpenseCareApi.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using ExpenseCareApi.Infrastructure.Repositories;

namespace ExpenseCareApi.Application.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepo;
    private readonly IMapper _mapper;

    public ExpenseService(IExpenseRepository expenseRepo, IMapper mapper)
    {
        _expenseRepo = expenseRepo;
        _mapper = mapper;
    }

    public async Task<GetExpenseDetailsDto> CreateAsync(CreateExpenseDto dto)
    {

        var entity = _mapper.Map<ExpenseDetails>(dto);

        entity.EntryAt = DateTime.UtcNow;
        //entity.EntryBy = _currentUser.Name;
        //entity.UserId = _currentUser.Id;
        entity.Status = "Pending";
        entity.EntryBy = string.IsNullOrWhiteSpace(dto.EntryBy) ? "Admin" : dto.EntryBy;


        var result = await _expenseRepo.CreateAsync(entity);
        return _mapper.Map<GetExpenseDetailsDto>(result);
    }
    public async Task<GetExpenseDetailsDto?> GetByIdAsync(int id)
    {
        var entity = await _expenseRepo.GetByIdAsync(id);

        if (entity == null)
            return null;

        var result = _mapper.Map<GetExpenseDetailsDto>(entity);

        return result;
    }

    public async Task<List<GetExpenseDetailsDto>> GetAllAsync()
    {
        var entities = await _expenseRepo.GetAllAsync();
        return _mapper.Map<List<GetExpenseDetailsDto>>(entities);

    }
    public async Task<List<GetExpenseDetailsDto>> GetByMonthAsync(int year, int month)
    {
        var entities = await _expenseRepo.GetByMonthAsync(year, month);
        return _mapper.Map<List<GetExpenseDetailsDto>>(entities);
    }
    public async Task<List<GetExpenseDetailsDto>> GetByYearAsync(int year)
    {
        var entities = await _expenseRepo.GetByYearAsync(year);
        return _mapper.Map<List<GetExpenseDetailsDto>>(entities);
    }
    public async Task<List<GetExpenseDetailsDto>> GetPendingAsync()
    {
        var entities = await _expenseRepo.GetPendingAsync();
        return _mapper.Map<List<GetExpenseDetailsDto>>(entities);
    }

    public async Task<bool> UpdateAsync(int id, UpdateExpenseDto dto)
    {
        var entity = await _expenseRepo.GetByIdAsync(id);

        if (entity == null)
            return false;

        _mapper.Map(dto, entity);

        await _expenseRepo.UpdateAsync(entity);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _expenseRepo.GetByIdAsync(id);

        if (entity == null)
            return false;

        await _expenseRepo.DeleteAsync(entity);
        return true;
    }

    public async Task<bool> ApproveAsync(int id, string approvedBy)
    {
        var entity = await _expenseRepo.GetByIdAsync(id);
        if (entity == null)
            return false;


        await _expenseRepo.ApproveAsync(id, approvedBy);
        return true;
    }

    public async Task<bool> RejectAsync(int id, string rejectedBy)
    {
        var entity = await _expenseRepo.GetByIdAsync(id);
        if (entity == null)
            return false;

        await _expenseRepo.RejectAsync(id, rejectedBy);
        return true;
    }
    public async Task<bool> ApproveAllAsync(List<int> ids, string approvedBy)
    {
        await _expenseRepo.ApproveAllAsync(ids, approvedBy);
        return true;
    }



}