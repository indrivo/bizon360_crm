using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using GR.Crm.Organizations.Abstractions.Models;
using GR.Crm.Organizations.Abstractions.ViewModels.OrganizationAddressViewModels;

namespace GR.Crm.Organizations.Abstractions.Helpers
{
    public class AddressMapperProfile : Profile
    {
        public AddressMapperProfile()
        {
            CreateMap<OrganizationAddress, OrganizationAddressViewModel>()
                .ForMember(o => o.OrganizationId, m => m.MapFrom(x => x.OrganizationId))
                .ForMember(o => o.CityId, m => m.MapFrom(x => x.CityId))
                .ForMember(o => o.Street, m => m.MapFrom(x => x.Street))
                .ForMember(o => o.Zip, m => m.MapFrom(x => x.Zip))
                .ReverseMap();


            CreateMap<OrganizationAddress, GetOrganizationAddressViewModel>()
                .IncludeAllDerived()
                .ReverseMap();



            CreateMap<Region, RegionViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.CountryId, m => m.MapFrom(x => x.CountryId))
                .ReverseMap();


            CreateMap<Region, GetRegionViewModel>()
                .IncludeAllDerived()
                .ReverseMap();



            CreateMap<City, CityViewModel>()
                .ForMember(o => o.Id, m => m.MapFrom(x => x.Id))
                .ForMember(o => o.IsDeleted, m => m.MapFrom(x => x.IsDeleted))
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.Region, m => m.MapFrom(x => x.Region))
                .ForMember(o => o.RegionId, m => m.MapFrom(x => x.RegionId))
                .ReverseMap();



            // mapped country
            CreateMap<CrmCountry, CountryViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ReverseMap();



            CreateMap<CrmCountry, GetCountryViewModel>()
                .IncludeAllDerived()
                .ReverseMap();

        }
    }
}
