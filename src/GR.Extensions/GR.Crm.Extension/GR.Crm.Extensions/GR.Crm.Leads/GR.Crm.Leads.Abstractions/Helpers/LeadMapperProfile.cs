using AutoMapper;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Leads.Abstractions.ViewModels;

namespace GR.Crm.Leads.Abstractions.Helpers
{
    public sealed class LeadMapperProfile : Profile
    {
        public LeadMapperProfile()
        {
            //Map create lead 
            CreateMap<Lead, AddLeadViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.CurrencyCode, m => m.MapFrom(x => x.CurrencyCode))
                .ForMember(o => o.OrganizationId, m => m.MapFrom(x => x.OrganizationId))
                .ForMember(o => o.PipeLineId, m => m.MapFrom(x => x.PipeLineId))
                .ForMember(o => o.StageId, m => m.MapFrom(x => x.StageId))
                .ForMember(o => o.Value, m => m.MapFrom(x => x.Value))
                .ForMember(o => o.DeadLine, m => m.MapFrom(x => x.DeadLine))
                .ForMember(o => o.LeadStateId, m => m.MapFrom(x => x.LeadStateId))
                .ForMember(o => o.ProductId, m => m.MapFrom(x => x.ProductId))
                .ForMember(o => o.ClarificationDeadline, m => m.MapFrom(x => x.ClarificationDeadline))
                .ForMember(o => o.ContactId, m => m.MapFrom(x => x.ContactId))
                .ForMember(o => o.ProductTypeId, m => m.MapFrom(x => x.ProductTypeId))
                .ForMember(o => o.ServiceTypeId, m => m.MapFrom(x => x.ServiceTypeId))
                .ForMember(o => o.SolutionTypeId, m => m.MapFrom(x => x.SolutionTypeId))
                .ForMember(o => o.SourceId, m => m.MapFrom(x => x.SourceId))
                .ForMember(o => o.TechnologyTypeId, m => m.MapFrom(x => x.TechnologyTypeId))
                .ForMember(o => o.Description, m => m.MapFrom(x => x.Description))
                .ReverseMap();

            //Map update lead 
            CreateMap<Lead, UpdateLeadViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.CurrencyCode, m => m.MapFrom(x => x.CurrencyCode))
                .ForMember(o => o.OrganizationId, m => m.MapFrom(x => x.OrganizationId))
                .ForMember(o => o.Id, m => m.MapFrom(x => x.Id))
                .ForMember(o => o.Value, m => m.MapFrom(x => x.Value))
                .ForMember(o => o.DeadLine, m => m.MapFrom(x => x.DeadLine))
                .ForMember(o => o.LeadStateId, m => m.MapFrom(x => x.LeadStateId))
                .ForMember(o => o.ProductId, m => m.MapFrom(x => x.ProductId))
                .ForMember(o => o.ClarificationDeadline, m => m.MapFrom(x => x.ClarificationDeadline))
                .ForMember(o => o.ContactId, m => m.MapFrom(x => x.ContactId))
                .ForMember(o => o.ProductTypeId, m => m.MapFrom(x => x.ProductTypeId))
                .ForMember(o => o.ServiceTypeId, m => m.MapFrom(x => x.ServiceTypeId))
                .ForMember(o => o.SolutionTypeId, m => m.MapFrom(x => x.SolutionTypeId))
                .ForMember(o => o.SourceId, m => m.MapFrom(x => x.SourceId))
                .ForMember(o => o.TechnologyTypeId, m => m.MapFrom(x => x.TechnologyTypeId))
                .ForMember(o => o.Description, m => m.MapFrom(x => x.Description))
                .ReverseMap();

            //Map lead with get viewmodel
            CreateMap<Lead, GetLeadsViewModel>()
                .IncludeAllDerived()
                .ReverseMap();
        }
    }
}
