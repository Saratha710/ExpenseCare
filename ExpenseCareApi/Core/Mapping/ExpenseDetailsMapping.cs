// Core/Mapping/ExpenseMappingProfile.cs
using AutoMapper;
using ExpenseCareApi.Core.DTOs;
using ExpenseCareApi.Core.Models;

namespace ExpenseCareApi.Core.Mapping;

public class ExpenseDetailsMapping : Profile
{
    public ExpenseDetailsMapping()
    {
        // CREATE: CreateExpenseDto → ExpenseDetails entity
        // ignore all server-set fields
        CreateMap<CreateExpenseDto, ExpenseDetails>()
            .ForMember(dest => dest.Id,         opt => opt.Ignore())
            .ForMember(dest => dest.UserId,     opt => opt.Ignore())
            .ForMember(dest => dest.EntryBy,    opt => opt.Ignore())
            .ForMember(dest => dest.EntryAt,    opt => opt.Ignore())
            .ForMember(dest => dest.Status,     opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedAt, opt => opt.Ignore());

        // UPDATE: UpdateExpenseDto → existing ExpenseDetails entity
        // same fields ignored — audit + approval never change on update
        CreateMap<UpdateExpenseDto, ExpenseDetails>()
            .ForMember(dest => dest.Id,         opt => opt.Ignore())
            .ForMember(dest => dest.UserId,     opt => opt.Ignore())
            .ForMember(dest => dest.EntryBy,    opt => opt.Ignore())
            .ForMember(dest => dest.EntryAt,    opt => opt.Ignore())
            .ForMember(dest => dest.Status,     opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedAt, opt => opt.Ignore());

        // GET: ExpenseDetails entity → GetExpenseDetailsDto
        // all field names match — AutoMapper handles automatically
        CreateMap<ExpenseDetails, GetExpenseDetailsDto>();

        // SUMMARY: ExpenseDetails → ExpenseResponseDto
        // computed fields mapped manually — for future dashboard use
        CreateMap<ExpenseDetails, ExpenseResponseDto>()
            .ForMember(dest => dest.StatusLabel,
                opt => opt.MapFrom(src =>
                    src.Status == "Approved" ? "✓ Approved" :
                    src.Status == "Pending"  ? "⏳ Pending"  :
                                               src.Status))
            .ForMember(dest => dest.CanEdit,
                opt => opt.MapFrom(src => src.Status == "Pending"));
    }
}