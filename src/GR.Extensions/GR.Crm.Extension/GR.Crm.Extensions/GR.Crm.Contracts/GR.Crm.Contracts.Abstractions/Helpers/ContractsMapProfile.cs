using AutoMapper;
using GR.Crm.Contracts.Abstractions.Models;
using GR.Crm.Contracts.Abstractions.ViewModels;

namespace GR.Crm.Contracts.Abstractions.Helpers
{
    public sealed class ContractsMapProfile : Profile
    {
        /// <summary>
        /// Mapping configurations
        /// </summary>
        public ContractsMapProfile()
        {
            //Map add contract template
            CreateMap<ContractTemplate, AddTemplateViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.Description, m => m.MapFrom(x => x.Description))
                .ReverseMap();

            //Map get section viewmodel
            CreateMap<ContractSection, GetTemplateSectionsViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.TemplateValue, m => m.MapFrom(x => x.TemplateValue))
                .ForMember(o => o.Order, m => m.MapFrom(x => x.Order))
                .ReverseMap();

            //Map update section viewmodel
            CreateMap<ContractSection, UpdateTemplateSectionViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.TemplateValue, m => m.MapFrom(x => x.TemplateValue))
                .ReverseMap();

            //Map add section viewmodel
            CreateMap<ContractSection, AddSectionViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.ContractTemplateId, m => m.MapFrom(x => x.ContractTemplateId))
                .ForMember(o => o.TemplateValue, m => m.MapFrom(x => x.TemplateValue))
                .ReverseMap();
        }
    }
}
