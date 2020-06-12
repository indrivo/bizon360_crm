using AutoMapper;
using GR.Crm.PipeLines.Abstractions.Models;
using GR.Crm.PipeLines.Abstractions.ViewModels;

namespace GR.Crm.PipeLines.Abstractions.Helpers
{
    public sealed class PipeLineMappingProfile : Profile
    {
        /// <summary>
        /// Configure mapping
        /// </summary>
        public PipeLineMappingProfile()
        {
            //Map create pipeline mapping
            CreateMap<PipeLine, CreatePipeLineViewModel>()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ForMember(o => o.Description, m => m.MapFrom(x => x.Description))
                .ForMember(o => o.WorkFlowId, m => m.MapFrom(x => x.WorkFlowId))
                .ReverseMap();
        }
    }
}
