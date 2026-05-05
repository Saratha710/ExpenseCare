using AutoMapper;
using ExpenseCareApi.Core.DTOs;
using ExpenseCareApi.Core.Models;

namespace ExpenseCareApi.Core.Mapping;

public class DonationDetailsMapping : Profile
{
    public DonationDetailsMapping()
    {
        CreateMap<CreateDonationDetailsDto, DonationDetails>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // DB generates
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src=> src.UserId)) // from JWT later
            .ForMember(dest => dest.EntryBy, opt => opt.Ignore()) // from JWT later
            .ForMember(dest => dest.EntryAt, opt => opt.Ignore()) // server sets
            .ForMember(dest => dest.Status, opt => opt.Ignore()) // server sets "Pending"
            .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore()) // approval flow only
            .ForMember(dest => dest.ApprovedAt, opt => opt.Ignore()); // approval flow only

        CreateMap<UpdateDonationDto, DonationDetails>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.EntryBy, opt => opt.Ignore())
            .ForMember(dest => dest.EntryAt, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedAt, opt => opt.Ignore());

        CreateMap<DonationDetails, GetDonationDetailsDto>();
        
        CreateMap<DonationDetails, DonationResponseDto>()
            .ForMember(dest => dest.StatusLabel,
                opt => opt.MapFrom(src =>
                    src.Status == "Approved" ? "✓ Approved" :
                    src.Status == "Pending"  ? "⏳ Pending"  :
                                               src.Status))
            .ForMember(dest => dest.CanEdit,
                opt => opt.MapFrom(src => src.Status == "Pending")); 

    }
}