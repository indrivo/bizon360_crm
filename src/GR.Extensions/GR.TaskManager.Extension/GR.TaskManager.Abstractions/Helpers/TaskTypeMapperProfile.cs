using AutoMapper;
using GR.TaskManager.Abstractions.Models;
using GR.TaskManager.Abstractions.Models.ViewModels.TaskTypeViewModels;

namespace GR.TaskManager.Abstractions.Helpers
{
    public class TaskTypeMapperProfile: Profile
    {

        public TaskTypeMapperProfile()
        {

            // TaskType 

            CreateMap<TaskType, TaskTypeViewModel> ()
                .ForMember(o => o.Name, m => m.MapFrom(x => x.Name))
                .ReverseMap();

            CreateMap<TaskType, GetTaskTypeViewModel>()
                .IncludeAllDerived()
                .ReverseMap();
        }

    }
}
