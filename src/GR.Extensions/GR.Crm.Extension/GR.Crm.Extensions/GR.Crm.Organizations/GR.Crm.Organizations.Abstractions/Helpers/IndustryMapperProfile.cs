using AutoMapper;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.IndustriesViewModels;

namespace GR.Crm.Organizations.Abstractions.Helpers
{
    public class IndustryMapperProfile: Profile
    {

        public IndustryMapperProfile()
        {

            CreateMap<Industry, GetIndustryViewModel>()
                .ForMember(o => o.Id, m => m.MapFrom(x => x.Id))
                .ForMember(o => o.IsDeleted, m => m.MapFrom(x => x.IsDeleted))
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ReverseMap();
        }

    }
}
