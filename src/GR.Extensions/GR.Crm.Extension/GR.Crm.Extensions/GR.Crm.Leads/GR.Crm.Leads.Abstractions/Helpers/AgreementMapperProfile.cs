using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Leads.Abstractions.ViewModels;
using GR.Crm.Leads.Abstractions.ViewModels.AgreementsViewModels;

namespace GR.Crm.Leads.Abstractions.Helpers
{
    public sealed class AgreementMapperProfile: Profile
    {
        public AgreementMapperProfile()
        {
            //Map create agreement 
            CreateMap<Agreement, AddAgreementViewModel>()
                .ForMember(o => o.LeadId, m => m.MapFrom(x => x.LeadId))
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.OrganizationId, m => m.MapFrom(x => x.OrganizationId))
                .ForMember(o => o.ContactId, m => m.MapFrom(x => x.ContactId))
                .ForMember(o => o.OrganizationAddressId, m => m.MapFrom(x => x.OrganizationAddressId))
                .ForMember(o => o.UserId, m => m.MapFrom(x => x.UserId))
                .ForMember(o => o.ContractTemplateId, m => m.MapFrom(x => x.ContractTemplateId))
                .ForMember(o => o.Commission, m => m.MapFrom(x => x.Commission))
                .ForMember(o => o.Values, m => m.MapFrom(x => x.Values))
                .ForMember(o => o.ProductId, m => m.MapFrom(x => x.ProductId))
                .ForMember(o=> o.Description, m=> m.MapFrom(x=> x.Description))
                .ReverseMap();

            //Map update agreement 
            CreateMap<Agreement, UpdateAgreementViewModel>()
                .ForMember(o => o.Id, m => m.MapFrom(x => x.Id))
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.LeadId, m => m.MapFrom(x => x.LeadId))
                .ForMember(o => o.OrganizationId, m => m.MapFrom(x => x.OrganizationId))
                .ForMember(o => o.ContactId, m => m.MapFrom(x => x.ContactId))
                .ForMember(o => o.OrganizationAddressId, m => m.MapFrom(x => x.OrganizationAddressId))
                .ForMember(o => o.UserId, m => m.MapFrom(x => x.UserId))
                .ForMember(o => o.ContractTemplateId, m => m.MapFrom(x => x.ContractTemplateId))
                .ForMember(o => o.Commission, m => m.MapFrom(x => x.Commission))
                .ForMember(o => o.Values, m => m.MapFrom(x => x.Values))
                .ForMember(o => o.ProductId, m => m.MapFrom(x => x.ProductId))
                .ForMember(o => o.Description, m => m.MapFrom(x => x.Description))
                .ReverseMap();

            //Map agreement with get viewmodel
            CreateMap<Agreement, GetAgreementViewModel>()
                .IncludeAllDerived()
                .ReverseMap();
        }
    }
}
