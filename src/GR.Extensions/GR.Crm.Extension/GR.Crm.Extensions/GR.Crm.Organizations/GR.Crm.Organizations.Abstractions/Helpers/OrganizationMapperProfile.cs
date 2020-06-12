using AutoMapper;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationsViewModels;

namespace GR.Crm.Organizations.Abstractions.Helpers
{
    public sealed class OrganizationMapperProfile : Profile
    {
        public OrganizationMapperProfile()
        {
            //Map create organization 
            CreateMap<Organization, OrganizationViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.Brand, m => m.MapFrom(x => x.Brand))
                .ForMember(o => o.ClientType, m => m.MapFrom(x => x.ClientType))
                .ForMember(o => o.Bank, m => m.MapFrom(x => x.Bank))
                .ForMember(o => o.Email, m => m.MapFrom(x => x.Email))
                .ForMember(o => o.Phone, m => m.MapFrom(x => x.Phone))
                .ForMember(o => o.WebSite, m => m.MapFrom(x => x.WebSite))
                .ForMember(o => o.FiscalCode, m => m.MapFrom(x => x.FiscalCode))
                .ForMember(o => o.IBANCode, m => m.MapFrom(x => x.IBANCode))
                .ForMember(o => o.CodSwift, m => m.MapFrom(x => x.CodSwift))
                .ForMember(o => o.VitCode, m => m.MapFrom(x => x.VitCode))
                .ForMember(o => o.Description, m => m.MapFrom(x => x.Description))
                .ForMember(o => o.IndustryId, m => m.MapFrom(x => x.IndustryId))
                .ForMember(o => o.EmployeeId, m => m.MapFrom(x => x.EmployeeId))
                .ReverseMap();


            //Map organization with get viewmodel
            CreateMap<Organization, GetOrganizationViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

        }
    }
}
